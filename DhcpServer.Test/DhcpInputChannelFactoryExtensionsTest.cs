// <copyright file="DhcpInputChannelFactoryExtensionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DhcpServer.Events;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DhcpInputChannelFactoryExtensionsTest
    {
        [TestMethod]
        public void WithEventsCreateChannel()
        {
            IList<string> events = TestWithEventsCreateChannel((f, e) => f.WithEvents(new StubInputChannelFactoryEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelStart(1, 500)",
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelEnd(1, True, <null>)");
        }

        [TestMethod]
        public void WithEventsCreateChannelState()
        {
            IList<string> events = TestWithEventsCreateChannel((f, e) => f.WithEvents(new StubInputChannelFactoryEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelStart(1, 500)",
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelEnd(1, True, <null>, State_1)");
        }

        [TestMethod]
        public void WithEventsCreateChannelWithException()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(new StubInputChannelFactoryEvents(events));

            Action act = () => outer.CreateChannel(new Memory<byte>(new byte[0]));

            act.Should().Throw<ArgumentOutOfRangeException>();
            events.Should().HaveCount(2).And.ContainInOrder(
                "CreateChannelStart(1, 0)",
                "CreateChannelEnd(1, False, ArgumentOutOfRangeException)");
        }

        [TestMethod]
        public void WithEventsCreateTwoChannels()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(new StubInputChannelFactoryEvents(events));

            IDhcpInputChannel channel1 = outer.CreateChannel(new Memory<byte>(new byte[500]));
            IDhcpInputChannel channel2 = outer.CreateChannel(new Memory<byte>(new byte[499]));

            channel1.Should().NotBeNull();
            channel2.Should().NotBeNull();
            channel1.Should().NotBeSameAs(channel2);
            events.Should().HaveCount(4).And.ContainInOrder(
                "CreateChannelStart(1, 500)",
                "CreateChannelEnd(1, True, <null>)",
                "CreateChannelStart(2, 499)",
                "CreateChannelEnd(2, True, <null>)");
        }

        [TestMethod]
        public void WithEventsReceive()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(null, new StubInputChannelEvents(events));

            using ActivityScope scope = new ActivityScope(new Guid("12345678-1234-5678-9abc-eeeeeeeeeeee"));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.None);
            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveStart(1)",
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveEnd(1, True, None, <null>)");
        }

        [TestMethod]
        public void WithEventsReceiveThrow()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(null, new StubInputChannelEvents(events));
            using CancellationTokenSource cts = new CancellationTokenSource();

            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            cts.Cancel();
            Task task = channel.ReceiveAsync(cts.Token);

            task.IsCanceled.Should().BeTrue();
            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, False, None, OperationCanceledException)");
        }

        [TestMethod]
        public void WithEventsReceiveErrorWithException()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(null, new StubInputChannelEvents(events));

            inner.Error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, False, SocketError, <null>)");
        }

        [TestMethod]
        public void WithEventsBothNull()
        {
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents();

            inner.Error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
        }

        private static IList<string> TestWithEventsCreateChannel(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);

            using ActivityScope scope = new ActivityScope(new Guid("12345678-1234-5678-9abc-ffffffffffff"));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            (DhcpMessageBuffer buffer, DhcpError error) = task.Result;
            buffer.Should().BeSameAs(inner.Buffer);
            error.Code.Should().Be(DhcpErrorCode.None);
            return events;
        }

        private static string ActivityPrefix()
        {
            Guid id = ActivityScope.CurrentId;
            if (id == Guid.Empty)
            {
                return string.Empty;
            }

            return id.ToString("D") + "/";
        }

        private sealed class StubInputChannelEvents : IDhcpInputChannelEvents
        {
            private readonly IList<string> events;

            public StubInputChannelEvents(IList<string> events)
            {
                this.events = events;
            }

            public void ReceiveStart(DhcpChannelId id)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.ReceiveStart)}({(int)id})");
            }

            public void ReceiveEnd(DhcpChannelId id, bool succeeded, DhcpError error, Exception exception)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.ReceiveEnd)}({(int)id}, {succeeded}, {error.Code}, {type})");
            }
        }

        private sealed class StubInputChannelFactoryEvents : IDhcpInputChannelFactoryEvents
        {
            private readonly IList<string> events;

            public StubInputChannelFactoryEvents(IList<string> events)
            {
                this.events = events;
            }

            public void CreateChannelStart(DhcpChannelId id, int bufferSize)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.CreateChannelStart)}({(int)id}, {bufferSize})");
            }

            public void CreateChannelEnd(DhcpChannelId id, bool succeeded, Exception exception)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.CreateChannelEnd)}({(int)id}, {succeeded}, {type})");
            }
        }

        private sealed class StubInputChannelFactoryEventsState : IDhcpInputChannelFactoryEvents<string>
        {
            private readonly IList<string> events;

            public StubInputChannelFactoryEventsState(IList<string> events)
            {
                this.events = events;
            }

            public string CreateChannelStart(DhcpChannelId id, int bufferSize)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.CreateChannelStart)}({(int)id}, {bufferSize})");
                return "State_" + (int)id;
            }

            public void CreateChannelEnd(DhcpChannelId id, bool succeeded, Exception exception, string state)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.CreateChannelEnd)}({(int)id}, {succeeded}, {type}, {state})");
            }
        }

        private sealed class StubInputChannelFactory : IDhcpInputChannelFactory
        {
            public DhcpMessageBuffer Buffer { get; private set; }

            public DhcpError Error { get; set; }

            public IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer)
            {
                this.Buffer = new DhcpMessageBuffer(rawBuffer);
                return new StubInputChannel(this.Buffer, this.Error);
            }

            private sealed class StubInputChannel : IDhcpInputChannel
            {
                private readonly DhcpMessageBuffer buffer;
                private readonly DhcpError error;

                public StubInputChannel(DhcpMessageBuffer buffer, DhcpError error)
                {
                    this.buffer = buffer;
                    this.error = error;
                }

                public Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult((this.buffer, this.error));
                }
            }
        }
    }
}
