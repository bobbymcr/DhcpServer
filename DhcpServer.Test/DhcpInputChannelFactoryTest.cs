// <copyright file="DhcpInputChannelFactoryTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DhcpInputChannelFactoryTest
    {
        [TestMethod]
        public void CreateAndReceiveFromTwo()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannelFactory factory = new DhcpInputChannelFactory(socket);

            IDhcpInputChannel channel1 = factory.CreateChannel(new Memory<byte>(new byte[500]));
            IDhcpInputChannel channel2 = factory.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task1 = channel1.ReceiveAsync(CancellationToken.None);
            Task<(DhcpMessageBuffer, DhcpError)> task2 = channel2.ReceiveAsync(CancellationToken.None);

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();

            Complete(socket, "Request1");

            task1.IsCompleted.Should().BeTrue();
            task2.IsCompleted.Should().BeFalse();
            (DhcpMessageBuffer buffer1, DhcpError error1) = task1.Result;
            error1.Code.Should().Be(DhcpErrorCode.None);
            buffer1.Opcode.Should().Be(DhcpOpcode.Request);
            buffer1.TransactionId.Should().Be(0x00003D1D);

            socket.Complete(new DhcpException(DhcpErrorCode.SocketError, null));

            task2.IsCompleted.Should().BeTrue();
            (_, DhcpError error2) = task2.Result;
            error2.Code.Should().Be(DhcpErrorCode.SocketError);
        }

        private static void Complete(StubInputSocket socket, string resourceName, int length = -1)
        {
            byte[] raw = new byte[66000];
            Span<byte> packet = new Memory<byte>(raw).Span;
            int actualLength = PacketResource.Read(resourceName, packet);
            if (length == -1)
            {
                length = actualLength;
            }

            socket.Complete(packet.Slice(0, length));
        }

        private sealed class StubInputSocket : IInputSocket
        {
            private readonly Queue<Tuple<Memory<byte>, TaskCompletionSource<int>>> pending;

            public StubInputSocket()
            {
                this.pending = new Queue<Tuple<Memory<byte>, TaskCompletionSource<int>>>();
            }

            public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                var next = Tuple.Create(buffer, new TaskCompletionSource<int>());
                this.pending.Enqueue(next);
                return new ValueTask<int>(next.Item2.Task);
            }

            public void Complete(ReadOnlySpan<byte> input)
            {
                var current = this.pending.Dequeue();
                Span<byte> output = current.Item1.Span;
                int safeLength = Math.Min(input.Length, output.Length);
                for (int i = 0; i < safeLength; ++i)
                {
                    output[i] = input[i];
                }

                current.Item2.SetResult(input.Length);
            }

            public void Complete(Exception exception)
            {
                this.pending.Dequeue().Item2.SetException(exception);
            }
        }
    }
}
