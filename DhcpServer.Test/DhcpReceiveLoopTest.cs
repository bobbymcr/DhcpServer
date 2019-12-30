// <copyright file="DhcpReceiveLoopTest.cs" company="Brian Rogers">
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
    public sealed class DhcpReceiveLoopTest
    {
        [TestMethod]
        public void RunAndReceiveTwo()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            byte[] raw = new byte[500];
            Memory<byte> inputBuffer = new Memory<byte>(raw);
            DhcpMessageBuffer[] messages = new DhcpMessageBuffer[1];
            static ValueTask ProcessAsync(DhcpMessageBuffer m, DhcpMessageBuffer[] a)
            {
                a[0] = m;
                return new ValueTask(Task.CompletedTask);
            }

            Task task = loop.RunAsync(inputBuffer, messages, CancellationToken.None, (m, a, t) => ProcessAsync(m, a));

            task.IsCompleted.Should().BeFalse();
            messages[0].Should().BeNull();

            Complete(socket, "Request1");

            task.IsCompleted.Should().BeFalse();
            messages[0].Should().NotBeNull();
            messages[0].Opcode.Should().Be(DhcpOpcode.Request);
            messages[0].TransactionId.Should().Be(0x00003D1D);

            messages[0] = null;
            Complete(socket, "Reply1");

            task.IsCompleted.Should().BeFalse();
            messages[0].Should().NotBeNull();
            messages[0].Opcode.Should().Be(DhcpOpcode.Reply);
            messages[0].TransactionId.Should().Be(0x3903F326);
        }

        [TestMethod]
        public void CancelRightAway()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            byte[] raw = new byte[500];
            Memory<byte> inputBuffer = new Memory<byte>(raw);
            int count = 0;
            ValueTask ShouldNotRun()
            {
                ++count;
                return new ValueTask(Task.CompletedTask);
            }

            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            Task task = loop.RunAsync(inputBuffer, new object(), cts.Token, (m, o, t) => ShouldNotRun());

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(0);
        }

        [TestMethod]
        public void CancelDuringProcess()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            byte[] raw = new byte[500];
            Memory<byte> inputBuffer = new Memory<byte>(raw);
            int count = 0;
            ValueTask ShouldBeCanceled(DhcpMessageBuffer m, CancellationTokenSource c, CancellationToken t)
            {
                ++count;
                if (m.Opcode == DhcpOpcode.Request)
                {
                    c.Cancel();
                }

                t.ThrowIfCancellationRequested();
                return new ValueTask(Task.CompletedTask);
            }

            using CancellationTokenSource cts = new CancellationTokenSource();
            Task task = loop.RunAsync(inputBuffer, cts, cts.Token, (m, c, t) => ShouldBeCanceled(m, c, t));

            task.IsCanceled.Should().BeFalse();

            Complete(socket, "Request1");

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(1);
        }

        private static void Complete(StubInputSocket socket, string resourceName)
        {
            byte[] raw = new byte[500];
            Span<byte> packet = new Memory<byte>(raw).Span;
            int length = PacketResource.Read(resourceName, packet);
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

            public void Complete(ReadOnlySpan<byte> data)
            {
                data.CopyTo(this.buffer.Span);
                this.next.SetResult(data.Length);
            }
        }
    }
}
