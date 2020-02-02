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
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(new byte[500]));
            DhcpError error = new DhcpError(new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner")));
            StubInputChannel inner = new StubInputChannel(buffer, error);
            IDhcpInputChannel outer = inner.WithEvents(1);

            Task<(DhcpMessageBuffer, DhcpError)> task = outer.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            task.Result.Item1.Should().BeSameAs(buffer);
            task.Result.Item2.Code.Should().Be(DhcpErrorCode.SocketError);
        }

        [TestMethod]
        public void WithEventsRestoresActivityIdOnEnd()
        {
            IList<string> events = TestWithEventsRestoresActivityIdOnEnd((c, e) => c.WithEvents(1, new StubInputChannelEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveStart(1)",
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveEnd(1, False, SocketError, <null>)");
        }

        [TestMethod]
        public void WithEventsRestoresActivityIdOnEndState()
        {
            IList<string> events = TestWithEventsRestoresActivityIdOnEnd((c, e) => c.WithEvents(1, new StubInputChannelEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveStart(1)",
                "12345678-1234-5678-9abc-cccccccccccc/ReceiveEnd(1, False, SocketError, <null>, State_1)");
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

        private sealed class StubInputChannelEventsState : IDhcpInputChannelEvents<string>
        {
            private readonly IList<string> events;

            public StubInputChannelEventsState(IList<string> events)
            {
                this.events = events;
            }

            public string ReceiveStart(DhcpChannelId id)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.ReceiveStart)}({(int)id})");
                return "State_" + (int)id;
            }

            public void ReceiveEnd(DhcpChannelId id, bool succeeded, DhcpError error, Exception exception, string state)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.ReceiveEnd)}({(int)id}, {succeeded}, {error.Code}, {type}, {state})");
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
