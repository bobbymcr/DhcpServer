// <copyright file="DhcpInputChannelTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DhcpInputChannelTest
    {
        [TestMethod]
        public void Receive()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            Complete(socket, "Request1");

            task.IsCompleted.Should().BeTrue();
            (DhcpMessageBuffer buffer, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.None);
            buffer.Opcode.Should().Be(DhcpOpcode.Request);
            buffer.TransactionId.Should().Be(0x00003D1D);
        }

        [TestMethod]
        public void CancelReceive()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            using CancellationTokenSource cts = new CancellationTokenSource();

            cts.Cancel();
            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(cts.Token);

            task.IsCanceled.Should().BeTrue();
        }

        [TestMethod]
        public void PacketTooSmall()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            Complete(socket, "Request1", 50);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooSmall);
        }

        [TestMethod]
        public void PacketTooLarge()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            Complete(socket, "LargeRequest1", 65536);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooLarge);
        }

        [TestMethod]
        public void PacketTooLargeForBuffer()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            Complete(socket, "LargeRequest1", 600);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooLarge);
        }

        [TestMethod]
        public void BufferTooSmall()
        {
            Memory<byte> rawBuffer = new Memory<byte>(new byte[50]);

            Action act = () => new DhcpInputChannel(new StubInputSocket(), rawBuffer);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ReceiveWithSocketError()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            Exception inner = new Exception();
            Exception exception = new DhcpException(DhcpErrorCode.SocketError, inner);

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete(exception);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.SocketError);
            error.Exception.Should().BeSameAs(exception);
            error.Exception.InnerException.Should().BeSameAs(inner);
        }

        [TestMethod]
        public void ReceiveWithUnhandledException()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            Exception exception = new InvalidOperationException("won't be handled");

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete(exception);

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
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
            private Memory<byte> buffer;
            private TaskCompletionSource<int> next;

            public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                token.ThrowIfCancellationRequested();
                this.buffer = buffer;
                this.next = new TaskCompletionSource<int>();
                return new ValueTask<int>(this.next.Task);
            }

            public void Complete(ReadOnlySpan<byte> input)
            {
                Span<byte> output = this.buffer.Span;
                int safeLength = Math.Min(input.Length, output.Length);
                for (int i = 0; i < safeLength; ++i)
                {
                    output[i] = input[i];
                }

                this.next.SetResult(input.Length);
            }

            public void Complete(Exception exception)
            {
                this.next.SetException(exception);
            }
        }
    }
}
