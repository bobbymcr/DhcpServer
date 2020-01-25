// <copyright file="DhcpReceiveLoopTest.cs" company="Brian Rogers">
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
    public sealed class DhcpReceiveLoopTest
    {
        [TestMethod]
        public void RunOneReceiveTwo()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) => count += (int)m.Opcode);

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            task.IsCompleted.Should().BeFalse();
            StubInputChannel channel = channelFactory.Channels.Should().ContainSingle().Which;

            channel.Complete("Request1");
            channel.Buffer.Opcode = DhcpOpcode.None;
            channel.Complete("Request1");

            task.IsCompleted.Should().BeFalse();
            count.Should().Be(2);
        }

        [TestMethod]
        public void RunTwoReceiveTwo()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) => count += (int)m.Opcode);

            Task task1 = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);
            Task task2 = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();
            channelFactory.Channels.Should().HaveCount(2);

            channelFactory.Channels[0].Complete("Request1");
            channelFactory.Channels[1].Complete("Request1");

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();
            count.Should().Be(2);
        }

        [TestMethod]
        public void ThrowOnSocketReceive()
        {
            Exception exception = new InvalidOperationException("unhandled");
            StubInputChannelFactory channelFactory = new StubInputChannelFactory(onReceive: () =>
            {
                throw exception;
            });
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks();
            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }

        [TestMethod]
        public void CancelRightAway()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) => count += (int)m.Opcode);
            using CancellationTokenSource cts = new CancellationTokenSource();

            cts.Cancel();
            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, cts.Token);

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(0);
        }

        [TestMethod]
        public void CancelOnReceive()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            StubInputChannelFactory channelFactory = new StubInputChannelFactory(onReceive: () => cts.Cancel());
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) =>
            {
                ++count;
                t.ThrowIfCancellationRequested();
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, cts.Token);

            channelFactory.Channels[0].Complete("Request1");

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(1);
        }

        [TestMethod]
        public void ThrowOnReceive()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            Exception exception = new InvalidOperationException("unhandled");
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) =>
            {
                throw exception;
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            channelFactory.Channels[0].Complete("Request1");

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }

        [TestMethod]
        public void Error()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            List<DhcpError> errors = new List<DhcpError>();
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onError: (e, t) => errors.Add(e));

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            channelFactory.Channels[0].Complete(new DhcpError(DhcpErrorCode.BufferError));

            task.IsCompleted.Should().BeFalse();
            errors.Should().ContainSingle().Which.Code.Should().Be(DhcpErrorCode.BufferError);
        }

        [TestMethod]
        public void CancelOnError()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            StubInputChannelFactory channelFactory = new StubInputChannelFactory(onReceive: () => cts.Cancel());
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onError: (e, t) =>
            {
                ++count;
                t.ThrowIfCancellationRequested();
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, cts.Token);

            channelFactory.Channels[0].Complete(new DhcpError(DhcpErrorCode.SocketError));

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(1);
        }

        [TestMethod]
        public void ThrowOnError()
        {
            StubInputChannelFactory channelFactory = new StubInputChannelFactory();
            DhcpReceiveLoop loop = new DhcpReceiveLoop(channelFactory);
            Exception exception = new InvalidOperationException("unhandled");
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onError: (e, t) =>
            {
                throw exception;
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            channelFactory.Channels[0].Complete(new DhcpError(DhcpErrorCode.SocketError));

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }

        private sealed class StubInputChannelFactory : IDhcpInputChannelFactory
        {
            private readonly Action onReceive;

            public StubInputChannelFactory(Action onReceive = null)
            {
                this.onReceive = onReceive ?? (() => { });
                this.Channels = new List<StubInputChannel>();
            }

            public IList<StubInputChannel> Channels { get; }

            public IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer)
            {
                StubInputChannel channel = new StubInputChannel(rawBuffer, this.onReceive);
                this.Channels.Add(channel);
                return channel;
            }
        }

        private sealed class StubInputChannel : IDhcpInputChannel
        {
            private readonly Action onReceive;

            private TaskCompletionSource<(DhcpMessageBuffer, DhcpError)> next;

            public StubInputChannel(Memory<byte> rawBuffer, Action onReceive)
            {
                this.Buffer = new DhcpMessageBuffer(rawBuffer);
                this.onReceive = onReceive;
            }

            public DhcpMessageBuffer Buffer { get; }

            public Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token)
            {
                token.ThrowIfCancellationRequested();
                this.onReceive();
                this.next = new TaskCompletionSource<(DhcpMessageBuffer, DhcpError)>();
                return this.next.Task;
            }

            public void Complete(string resourceName)
            {
                byte[] raw = new byte[66000];
                Span<byte> packet = new Memory<byte>(raw).Span;
                ushort actualLength = PacketResource.Read(resourceName, packet);
                packet.Slice(0, actualLength).CopyTo(this.Buffer.Span);
                this.Buffer.Load(actualLength);
                this.next.SetResult((this.Buffer, default));
            }

            public void Complete(DhcpError error)
            {
                this.next.SetResult((this.Buffer, error));
            }
        }

        private sealed class StubDhcpReceiveCallbacks : IDhcpReceiveCallbacks
        {
            private static readonly ValueTask Completed = new ValueTask(Task.CompletedTask);

            private readonly Action<DhcpMessageBuffer, CancellationToken> onReceive;
            private readonly Action<DhcpError, CancellationToken> onError;

            public StubDhcpReceiveCallbacks(
                Action<DhcpMessageBuffer, CancellationToken> onReceive = null,
                Action<DhcpError, CancellationToken> onError = null)
            {
                this.onReceive = onReceive ?? ((m, t) => { });
                this.onError = onError ?? ((e, t) => { });
            }

            public ValueTask OnErrorAsync(DhcpError error, CancellationToken token)
            {
                this.onError(error, token);
                return Completed;
            }

            public ValueTask OnReceiveAsync(DhcpMessageBuffer message, CancellationToken token)
            {
                this.onReceive(message, token);
                return Completed;
            }
        }
    }
}
