// <copyright file="DhcpInputChannelExtensionsTest.cs" company="Brian Rogers">
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
    public sealed class DhcpInputChannelExtensionsTest
    {
        [TestMethod]
        public void WithEventsNull()
        {
            TestWithEventsNull(c => c.WithEvents(1));
        }

        [TestMethod]
        public void WithEventsNullState()
        {
            TestWithEventsNull(c => c.WithEvents<string>(1));
        }

        [TestMethod]
        public void WithEventsRestoresActivityIdOnEnd()
        {
            IList<string> events = TestWithEventsRestoresActivityIdOnEnd((c, e) => c.WithEvents(1, new StubInputChannelEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveStart(1)",
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveEnd(1, SocketError, Failed[<null>])");
        }

        [TestMethod]
        public void WithEventsRestoresActivityIdOnEndState()
        {
            IList<string> events = TestWithEventsRestoresActivityIdOnEnd((c, e) => c.WithEvents(1, new StubInputChannelEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveStart(1)",
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveEnd(1, SocketError, Failed[<null>], State_1)");
        }

        private static void TestWithEventsNull(Func<IDhcpInputChannel, IDhcpInputChannel> init)
        {
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(new byte[500]));
            DhcpError error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            StubInputChannel inner = new StubInputChannel(buffer, error);
            IDhcpInputChannel outer = init(inner);

            Task<(DhcpMessageBuffer, DhcpError)> task = outer.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item1.Should().BeSameAs(buffer);
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
        }

        private static IList<string> TestWithEventsRestoresActivityIdOnEnd(Func<IDhcpInputChannel, IList<string>, IDhcpInputChannel> init)
        {
            List<string> events = new List<string>();
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(new byte[500]));
            DhcpError error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            StubInputChannel inner = new StubInputChannel(buffer, error);
            IDhcpInputChannel outer = init(inner, events);

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-cccccccccccc"));
            inner.Pending = new TaskCompletionSource<(DhcpMessageBuffer, DhcpError)>();
            Task<(DhcpMessageBuffer, DhcpError)> task = outer.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-dddddddddddd"));
            inner.Pending.SetResult((buffer, error));

            task.IsCompleted.Should().BeTrue();
            task.Result.Item1.Should().BeSameAs(buffer);
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
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

        private sealed class StubInputChannel : IDhcpInputChannel
        {
            private readonly DhcpMessageBuffer buffer;
            private readonly DhcpError error;

            public StubInputChannel(DhcpMessageBuffer buffer, DhcpError error)
            {
                this.buffer = buffer;
                this.error = error;
            }

            public TaskCompletionSource<(DhcpMessageBuffer, DhcpError)> Pending { get; set; }

            public Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token)
            {
                if (this.Pending == null)
                {
                    return Task.FromResult((this.buffer, this.error));
                }

                return this.Pending.Task;
            }
        }
    }
}
