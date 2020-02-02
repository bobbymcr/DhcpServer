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
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelEnd(1, Succeeded)");
        }

        [TestMethod]
        public void WithEventsCreateChannelState()
        {
            IList<string> events = TestWithEventsCreateChannel((f, e) => f.WithEvents(new StubInputChannelFactoryEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelStart(1, 500)",
                "12345678-1234-5678-9abc-ffffffffffff/CreateChannelEnd(1, Succeeded, State_1)");
        }

        [TestMethod]
        public void WithEventsCreateChannelWithException()
        {
            IList<string> events = TestWithEventsCreateChannelWithException((f, e) => f.WithEvents(new StubInputChannelFactoryEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "CreateChannelStart(1, 0)",
                "CreateChannelEnd(1, Failed[ArgumentOutOfRangeException])");
        }

        [TestMethod]
        public void WithEventsCreateChannelWithExceptionState()
        {
            IList<string> events = TestWithEventsCreateChannelWithException((f, e) => f.WithEvents(new StubInputChannelFactoryEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "CreateChannelStart(1, 0)",
                "CreateChannelEnd(1, Failed[ArgumentOutOfRangeException], State_1)");
        }

        [TestMethod]
        public void WithEventsCreateTwoChannels()
        {
            IList<string> events = TestWithEventsCreateTwoChannels((f, e) => f.WithEvents(new StubInputChannelFactoryEvents(e)));

            events.Should().HaveCount(4).And.ContainInOrder(
                "CreateChannelStart(1, 500)",
                "CreateChannelEnd(1, Succeeded)",
                "CreateChannelStart(2, 499)",
                "CreateChannelEnd(2, Succeeded)");
        }

        [TestMethod]
        public void WithEventsCreateTwoChannelsState()
        {
            IList<string> events = TestWithEventsCreateTwoChannels((f, e) => f.WithEvents(new StubInputChannelFactoryEventsState(e)));

            events.Should().HaveCount(4).And.ContainInOrder(
                "CreateChannelStart(1, 500)",
                "CreateChannelEnd(1, Succeeded, State_1)",
                "CreateChannelStart(2, 499)",
                "CreateChannelEnd(2, Succeeded, State_2)");
        }

        [TestMethod]
        public void WithEventsReceive()
        {
            IList<string> events = TestWithEventsReceive((f, e) => f.WithEvents(null, new StubInputChannelEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveStart(1)",
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveEnd(1, None, Succeeded)");
        }

        [TestMethod]
        public void WithEventsReceiveState()
        {
            IList<string> events = TestWithEventsReceive((f, e) => f.WithEvents(null, new StubInputChannelEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveStart(1)",
                "12345678-1234-5678-9abc-eeeeeeeeeeee/ReceiveEnd(1, None, Succeeded, State_1)");
        }

        [TestMethod]
        public void WithEventsReceiveThrow()
        {
            IList<string> events = TestWithEventsReceiveThrow((f, e) => f.WithEvents(null, new StubInputChannelEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, None, Failed[OperationCanceledException])");
        }

        [TestMethod]
        public void WithEventsReceiveThrowState()
        {
            IList<string> events = TestWithEventsReceiveThrow((f, e) => f.WithEvents(null, new StubInputChannelEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, None, Failed[OperationCanceledException], State_1)");
        }

        [TestMethod]
        public void WithEventsReceiveErrorWithException()
        {
            IList<string> events = TestWithEventsReceiveErrorWithException((f, e) => f.WithEvents(null, new StubInputChannelEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, SocketError, Failed[<null>])");
        }

        [TestMethod]
        public void WithEventsReceiveErrorWithExceptionState()
        {
            IList<string> events = TestWithEventsReceiveErrorWithException((f, e) => f.WithEvents(null, new StubInputChannelEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "ReceiveStart(1)",
                "ReceiveEnd(1, SocketError, Failed[<null>], State_1)");
        }

        [TestMethod]
        public void WithEventsBothNull()
        {
            TestWithEventsBothNull(f => f.WithEvents());
        }

        [TestMethod]
        public void WithEventsBothNullState()
        {
            TestWithEventsBothNull(f => f.WithEvents<string>());
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

        private static IList<string> TestWithEventsCreateChannelWithException(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);

            Action act = () => outer.CreateChannel(new Memory<byte>(new byte[0]));

            act.Should().Throw<ArgumentOutOfRangeException>();
            return events;
        }

        private static IList<string> TestWithEventsCreateTwoChannels(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);

            IDhcpInputChannel channel1 = outer.CreateChannel(new Memory<byte>(new byte[500]));
            IDhcpInputChannel channel2 = outer.CreateChannel(new Memory<byte>(new byte[499]));

            channel1.Should().NotBeNull();
            channel2.Should().NotBeNull();
            channel1.Should().NotBeSameAs(channel2);
            return events;
        }

        private static IList<string> TestWithEventsReceive(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);

            using ActivityScope scope = new ActivityScope(new Guid("12345678-1234-5678-9abc-eeeeeeeeeeee"));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.None);
            return events;
        }

        private static IList<string> TestWithEventsReceiveThrow(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);
            using CancellationTokenSource cts = new CancellationTokenSource();

            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            cts.Cancel();
            Task task = channel.ReceiveAsync(cts.Token);

            task.IsCanceled.Should().BeTrue();
            return events;
        }

        private static IList<string> TestWithEventsReceiveErrorWithException(Func<IDhcpInputChannelFactory, IList<string>, IDhcpInputChannelFactory> init)
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner, events);

            inner.Error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
            return events;
        }

        private static void TestWithEventsBothNull(Func<IDhcpInputChannelFactory, IDhcpInputChannelFactory> init)
        {
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = init(inner);

            inner.Error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
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

        private static string StatusText(OperationStatus status)
        {
            if (status.Succeeded)
            {
                return "Succeeded";
            }

            string type = (status.Exception != null) ? status.Exception.GetType().Name : "<null>";
            return $"Failed[{type}]";
        }

        private abstract class StubInputChannelEventsBase
        {
            private readonly IList<string> events;

            protected StubInputChannelEventsBase(IList<string> events)
            {
                this.events = events;
            }

            public string ReceiveStartBase(DhcpChannelId id)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(IDhcpInputChannelEvents.ReceiveStart)}({(int)id})");
                return "State_" + (int)id;
            }

            public void ReceiveEndBase(DhcpChannelId id, DhcpError error, OperationStatus status, string state)
            {
                string prefix = ActivityPrefix();
                string statusText = StatusText(status);
                string suffix = (state != null) ? $", {state}" : string.Empty;
                this.events.Add($"{prefix}{nameof(IDhcpInputChannelEvents.ReceiveEnd)}({(int)id}, {error.Code}, {statusText}{suffix})");
            }
        }

        private sealed class StubInputChannelEvents : StubInputChannelEventsBase, IDhcpInputChannelEvents
        {
            public StubInputChannelEvents(IList<string> events)
                : base(events)
            {
            }

            public void ReceiveStart(DhcpChannelId id) => this.ReceiveStartBase(id);

            public void ReceiveEnd(DhcpChannelId id, DhcpError error, OperationStatus status)
            {
                this.ReceiveEndBase(id, error, status, null);
            }
        }

        private sealed class StubInputChannelEventsState : StubInputChannelEventsBase, IDhcpInputChannelEvents<string>
        {
            public StubInputChannelEventsState(IList<string> events)
                : base(events)
            {
            }

            public string ReceiveStart(DhcpChannelId id) => this.ReceiveStartBase(id);

            public void ReceiveEnd(DhcpChannelId id, DhcpError error, OperationStatus status, string state)
            {
                this.ReceiveEndBase(id, error, status, state);
            }
        }

        private abstract class StubInputChannelFactoryEventsBase
        {
            private readonly IList<string> events;

            protected StubInputChannelFactoryEventsBase(IList<string> events)
            {
                this.events = events;
            }

            public string CreateChannelStartBase(DhcpChannelId id, int bufferSize)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(IDhcpInputChannelFactoryEvents.CreateChannelStart)}({(int)id}, {bufferSize})");
                return "State_" + (int)id;
            }

            public void CreateChannelEndBase(DhcpChannelId id, OperationStatus status, string state)
            {
                string prefix = ActivityPrefix();
                string statusText = StatusText(status);
                string suffix = (state != null) ? $", {state}" : string.Empty;
                this.events.Add($"{prefix}{nameof(IDhcpInputChannelFactoryEvents.CreateChannelEnd)}({(int)id}, {statusText}{suffix})");
            }
        }

        private sealed class StubInputChannelFactoryEvents : StubInputChannelFactoryEventsBase, IDhcpInputChannelFactoryEvents
        {
            public StubInputChannelFactoryEvents(IList<string> events)
                : base(events)
            {
            }

            public void CreateChannelStart(DhcpChannelId id, int bufferSize) => this.CreateChannelStartBase(id, bufferSize);

            public void CreateChannelEnd(DhcpChannelId id, OperationStatus status)
            {
                this.CreateChannelEndBase(id, status, null);
            }
        }

        private sealed class StubInputChannelFactoryEventsState : StubInputChannelFactoryEventsBase, IDhcpInputChannelFactoryEvents<string>
        {
            public StubInputChannelFactoryEventsState(IList<string> events)
                : base(events)
            {
            }

            public string CreateChannelStart(DhcpChannelId id, int bufferSize) => this.CreateChannelStartBase(id, bufferSize);

            public void CreateChannelEnd(DhcpChannelId id, OperationStatus status, string state)
            {
                this.CreateChannelEndBase(id, status, state);
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
