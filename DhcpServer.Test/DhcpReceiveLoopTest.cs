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

        [TestMethod]
        public void PacketTooSmall()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            byte[] raw = new byte[500];
            Memory<byte> inputBuffer = new Memory<byte>(raw);
            int receiveCount = 0;
            DhcpError error = default;
            using CancellationTokenSource cts = new CancellationTokenSource();
            void ErrorCallback(DhcpError e, CancellationToken t)
            {
                error = e;
                cts.Cancel();
                t.ThrowIfCancellationRequested();
            }

            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks((m, t) => ++receiveCount, ErrorCallback);

            Task task = loop.RunAsync(inputBuffer, callbacks, cts.Token);

            task.IsCompleted.Should().BeFalse();

            error.Code.Should().Be(DhcpErrorCode.None);

            Complete(socket, "Request1", 50);

            task.IsCanceled.Should().BeTrue();
            receiveCount.Should().Be(0);
            error.Code.Should().Be(DhcpErrorCode.PacketTooSmall);
        }

        [TestMethod]
        public void BufferTooSmall()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            int count = 0;
            Task task;

            Action act = () => task = loop.RunAsync(new Memory<byte>(new byte[1]), new StubDhcpReceiveCallbacks((m, t) => ++count), CancellationToken.None);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ReceiveWithSocketError()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            int count = 0;
            DhcpError error = default;
            using CancellationTokenSource cts = new CancellationTokenSource();
            void ErrorCallback(DhcpError e, CancellationToken t)
            {
                error = e;
                cts.Cancel();
                t.ThrowIfCancellationRequested();
            }

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), new StubDhcpReceiveCallbacks((m, t) => ++count, ErrorCallback), cts.Token);

            task.IsCompleted.Should().BeFalse();
            error.Code.Should().Be(DhcpErrorCode.None);

            Exception exception = new DhcpException(DhcpErrorCode.SocketError);
            socket.Complete(exception);

            count.Should().Be(0);
            error.Code.Should().Be(DhcpErrorCode.SocketError);
            error.Exception.Should().BeSameAs(exception);
            task.IsCanceled.Should().BeTrue();
        }

        [TestMethod]
        public void ReceiveWithGenericCallbackException()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            Exception exception = new Exception();
            void Callback(DhcpMessageBuffer m, CancellationToken t)
            {
                if (m.Opcode == DhcpOpcode.Request)
                {
                    throw exception;
                }
            }

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), new StubDhcpReceiveCallbacks(Callback), CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            Complete(socket, "Request1");

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }

        [TestMethod]
        public void ReceiveWithGenericReceiveException()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(socket);
            Exception exception = new Exception();
            int count = 0;
            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), new StubDhcpReceiveCallbacks((m, t) => ++count), CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete(exception);

            count.Should().Be(0);
            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }

        private static void Complete(StubInputSocket socket, string resourceName, int length = -1)
        {
            byte[] raw = new byte[500];
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

        private sealed class StubDhcpReceiveCallbacks : IDhcpReceiveCallbacks
        {
            private readonly Action<DhcpMessageBuffer, CancellationToken> onReceive;
            private readonly Action<DhcpError, CancellationToken> onError;

            public StubDhcpReceiveCallbacks(Action<DhcpMessageBuffer, CancellationToken> onReceive, Action<DhcpError, CancellationToken> onError = null)
            {
                this.onReceive = onReceive;
                this.onError = onError;
            }

            public ValueTask OnReceiveAsync(DhcpMessageBuffer message, CancellationToken token)
            {
                this.onReceive(message, token);
                return new ValueTask(Task.CompletedTask);
            }

            public ValueTask OnErrorAsync(DhcpError error, CancellationToken token)
            {
                this.onError(error, token);
                return new ValueTask(Task.CompletedTask);
            }
        }
    }
}
