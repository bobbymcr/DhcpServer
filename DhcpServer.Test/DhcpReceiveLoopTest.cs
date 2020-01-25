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
            List<StubInputChannel> channels = new List<StubInputChannel>();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b);
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) => count += (int)m.Opcode);

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            task.IsCompleted.Should().BeFalse();
            StubInputChannel channel = channels.Should().ContainSingle().Which;

            channel.Complete("Request1");
            channel.Buffer.Opcode = DhcpOpcode.None;
            channel.Complete("Request1");

            task.IsCompleted.Should().BeFalse();
            count.Should().Be(2);
        }

        [TestMethod]
        public void RunTwoReceiveTwo()
        {
            List<StubInputChannel> channels = new List<StubInputChannel>();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b);
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) => count += (int)m.Opcode);

            Task task1 = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);
            Task task2 = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();
            channels.Should().HaveCount(2);

            channels[0].Complete("Request1");
            channels[1].Complete("Request1");

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();
            count.Should().Be(2);
        }

        [TestMethod]
        public void CancelRightAway()
        {
            List<StubInputChannel> channels = new List<StubInputChannel>();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b);
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
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
            List<StubInputChannel> channels = new List<StubInputChannel>();
            using CancellationTokenSource cts = new CancellationTokenSource();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b, onReceive: () => cts.Cancel());
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onReceive: (m, t) =>
            {
                ++count;
                t.ThrowIfCancellationRequested();
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, cts.Token);

            channels[0].Complete("Request1");

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(1);
        }

        [TestMethod]
        public void Error()
        {
            List<StubInputChannel> channels = new List<StubInputChannel>();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b);
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
            List<DhcpError> errors = new List<DhcpError>();
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onError: (e, t) => errors.Add(e));

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, CancellationToken.None);

            channels[0].Complete(new DhcpError(DhcpErrorCode.BufferError));

            task.IsCompleted.Should().BeFalse();
            errors.Should().ContainSingle().Which.Code.Should().Be(DhcpErrorCode.BufferError);
        }

        [TestMethod]
        public void CancelOnError()
        {
            List<StubInputChannel> channels = new List<StubInputChannel>();
            using CancellationTokenSource cts = new CancellationTokenSource();
            Func<Memory<byte>, IDhcpInputChannel> createChannel = b =>
            {
                StubInputChannel channel = new StubInputChannel(b, onReceive: () => cts.Cancel());
                channels.Add(channel);
                return channel;
            };
            DhcpReceiveLoop loop = new DhcpReceiveLoop(createChannel);
            int count = 0;
            StubDhcpReceiveCallbacks callbacks = new StubDhcpReceiveCallbacks(onError: (e, t) =>
            {
                ++count;
                t.ThrowIfCancellationRequested();
            });

            Task task = loop.RunAsync(new Memory<byte>(new byte[500]), callbacks, cts.Token);

            channels[0].Complete(new DhcpError(DhcpErrorCode.SocketError));

            task.IsCanceled.Should().BeTrue();
            count.Should().Be(1);
        }

        private sealed class StubInputChannel : IDhcpInputChannel
        {
            private readonly Action onReceive;

            private TaskCompletionSource<(DhcpMessageBuffer, DhcpError)> next;

            public StubInputChannel(Memory<byte> rawBuffer, Action onReceive = null)
            {
                this.Buffer = new DhcpMessageBuffer(rawBuffer);
                this.onReceive = onReceive ?? (() => { });
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
