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
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks((m, t) => messages[0] = m);

            Task task = loop.RunAsync(inputBuffer, callbacks, CancellationToken.None);

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
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks((m, t) => ++count);

            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            Task task = loop.RunAsync(inputBuffer, callbacks, cts.Token);

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(0);
        }

        [TestMethod]
        public void CancelDuringProcess()
        {
            int count = 0;
            using CancellationTokenSource cts = new CancellationTokenSource();
            void Callback(DhcpMessageBuffer m, CancellationToken t)
            {
                ++count;
                if (m.Opcode == DhcpOpcode.Request)
                {
                    cts.Cancel();
                }

                t.ThrowIfCancellationRequested();
            }

            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            byte[] raw = new byte[500];
            Memory<byte> inputBuffer = new Memory<byte>(raw);
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(Callback);

            Task task = loop.RunAsync(inputBuffer, callbacks, cts.Token);

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

        private sealed class StubDhcpReceiveCallbacks : IDhcpReceiveCallbacks
        {
            private readonly Action<DhcpMessageBuffer, CancellationToken> onReceive;

            public StubDhcpReceiveCallbacks(Action<DhcpMessageBuffer, CancellationToken> onReceive)
            {
                this.onReceive = onReceive;
            }

            public ValueTask OnReceiveAsync(DhcpMessageBuffer message, CancellationToken token)
            {
                this.onReceive(message, token);
                return new ValueTask(Task.CompletedTask);
            }
        }
    }
}
