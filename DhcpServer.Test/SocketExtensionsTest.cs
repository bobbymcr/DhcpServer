// <copyright file="SocketExtensionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DhcpServer;
    using DhcpServer.Events;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class SocketExtensionsTest
    {
        [TestMethod]
        public void WithEventsSend()
        {
            IList<string> events = TestWithEventsSend((s, e) => s.WithEvents(1, new StubSocketEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000011110000/SendStart(1, 500, 0x04030201, 5566)",
                "12345678-1234-5678-9abc-000011110000/SendEnd(1, Succeeded)");
        }

        [TestMethod]
        public void WithEventsSendState()
        {
            IList<string> events = TestWithEventsSend((s, e) => s.WithEvents(1, new StubSocketEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000011110000/SendStart(1, 500, 0x04030201, 5566)",
                "12345678-1234-5678-9abc-000011110000/SendEnd(1, Succeeded, State_1)");
        }

        [TestMethod]
        public void WithEventsSendException()
        {
            IList<string> events = TestWithEventsSendException((s, e) => s.WithEvents(2, new StubSocketEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000022220000/SendStart(2, 499, 0x99887766, 443)",
                "12345678-1234-5678-9abc-000022220000/SendEnd(2, Failed[DhcpException])");
        }

        [TestMethod]
        public void WithEventsSendExceptionState()
        {
            IList<string> events = TestWithEventsSendException((s, e) => s.WithEvents(2, new StubSocketEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000022220000/SendStart(2, 499, 0x99887766, 443)",
                "12345678-1234-5678-9abc-000022220000/SendEnd(2, Failed[DhcpException], State_2)");
        }

        [TestMethod]
        public void WithEventsReceive()
        {
            IList<string> events = TestWithEventsReceive((s, e) => s.WithEvents(1, new StubSocketEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000033330000/ReceiveStart(1, 500)",
                "12345678-1234-5678-9abc-000033330000/ReceiveEnd(1, 77, Succeeded)");
        }

        [TestMethod]
        public void WithEventsReceiveState()
        {
            IList<string> events = TestWithEventsReceive((s, e) => s.WithEvents(1, new StubSocketEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000033330000/ReceiveStart(1, 500)",
                "12345678-1234-5678-9abc-000033330000/ReceiveEnd(1, 77, Succeeded, State_1)");
        }

        [TestMethod]
        public void WithEventsReceiveException()
        {
            IList<string> events = TestWithEventsReceiveException((s, e) => s.WithEvents(2, new StubSocketEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000044440000/ReceiveStart(2, 499)",
                "12345678-1234-5678-9abc-000044440000/ReceiveEnd(2, -1, Failed[TaskCanceledException])");
        }

        [TestMethod]
        public void WithEventsReceiveExceptionState()
        {
            IList<string> events = TestWithEventsReceiveException((s, e) => s.WithEvents(2, new StubSocketEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000044440000/ReceiveStart(2, 499)",
                "12345678-1234-5678-9abc-000044440000/ReceiveEnd(2, -1, Failed[TaskCanceledException], State_2)");
        }

        [TestMethod]
        public void WithEventsDispose()
        {
            IList<string> events = TestWithEventsDispose((s, e) => s.WithEvents(3, new StubSocketEvents(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "DisposeStart(3)",
                "DisposeEnd(3)");
        }

        [TestMethod]
        public void WithEventsDisposeState()
        {
            IList<string> events = TestWithEventsDispose((s, e) => s.WithEvents(3, new StubSocketEventsState(e)));

            events.Should().HaveCount(2).And.ContainInOrder(
                "DisposeStart(3)",
                "DisposeEnd(3, State_3)");
        }

        private static IList<string> TestWithEventsSend(Func<ISocket, IList<string>, ISocket> init)
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = init(inner, events);
            IPEndpointV4 endpoint = new IPEndpointV4(new IPAddressV4(1, 2, 3, 4), 5566);
            ReadOnlyMemory<byte> buffer = new ReadOnlyMemory<byte>(new byte[500]);

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000011110000"));
            Task task = outer.SendAsync(buffer, endpoint);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            inner.Complete();

            task.IsCompleted.Should().BeTrue();
            return events;
        }

        private static IList<string> TestWithEventsSendException(Func<ISocket, IList<string>, ISocket> init)
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = init(inner, events);
            IPEndpointV4 endpoint = new IPEndpointV4(new IPAddressV4(0x66, 0x77, 0x88, 0x99), 443);
            ReadOnlyMemory<byte> buffer = new ReadOnlyMemory<byte>(new byte[499]);
            Exception exception = new DhcpException(DhcpErrorCode.SocketError, new InvalidOperationException("inner"));

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000022220000"));
            Task task = outer.SendAsync(buffer, endpoint);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            inner.Complete(exception);

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
            return events;
        }

        private static IList<string> TestWithEventsReceive(Func<ISocket, IList<string>, ISocket> init)
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = init(inner, events);
            Memory<byte> buffer = new Memory<byte>(new byte[500]);

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000033330000"));
            ValueTask<int> task = outer.ReceiveAsync(buffer, CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            inner.Complete(77);

            task.IsCompleted.Should().BeTrue();
            task.Result.Should().Be(77);
            return events;
        }

        private static IList<string> TestWithEventsReceiveException(Func<ISocket, IList<string>, ISocket> init)
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = init(inner, events);
            Memory<byte> buffer = new Memory<byte>(new byte[499]);

            using CancellationTokenSource cts = new CancellationTokenSource();
            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000044440000"));
            ValueTask<int> task = outer.ReceiveAsync(buffer, cts.Token);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            cts.Cancel();

            task.IsCanceled.Should().BeTrue();
            return events;
        }

        private static IList<string> TestWithEventsDispose(Func<ISocket, IList<string>, ISocket> init)
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = init(inner, events);

            outer.Dispose();

            inner.DisposeCount.Should().Be(1);
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

        private sealed class StubSocket : ISocket
        {
            private TaskCompletionSource<int> pending;

            public int DisposeCount { get; private set; }

            public void Complete() => this.Complete(0);

            public void Complete(int result) => this.pending.SetResult(result);

            public void Complete(Exception exception) => this.pending.SetException(exception);

            public Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint)
            {
                this.pending = new TaskCompletionSource<int>();
                return this.pending.Task;
            }

            public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                this.pending = new TaskCompletionSource<int>();
                token.Register(() => this.pending.SetCanceled());
                return new ValueTask<int>(this.pending.Task);
            }

            public void Dispose() => ++this.DisposeCount;
        }

        private abstract class StubSocketEventsBase
        {
            private readonly IList<string> events;

            protected StubSocketEventsBase(IList<string> events)
            {
                this.events = events;
            }

            public string SendStartBase(SocketId id, int bufferSize, IPEndpointV4 endpoint)
            {
                string prefix = ActivityPrefix();
                this.events.Add(
                    $"{prefix}{nameof(ISocketEvents.SendStart)}({(int)id}, {bufferSize}, 0x{((uint)endpoint.Address).ToString("X8")}, {endpoint.Port})");
                return "State_" + (int)id;
            }

            public void SendEndBase(SocketId id, OperationStatus status, string state)
            {
                string prefix = ActivityPrefix();
                string statusText = StatusText(status);
                string suffix = (state != null) ? $", {state}" : string.Empty;
                this.events.Add($"{prefix}{nameof(ISocketEvents.SendEnd)}({(int)id}, {statusText}{suffix})");
            }

            public string ReceiveStartBase(SocketId id, int bufferSize)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(ISocketEvents.ReceiveStart)}({(int)id}, {bufferSize})");
                return "State_" + (int)id;
            }

            public void ReceiveEndBase(SocketId id, int result, OperationStatus status, string state)
            {
                string prefix = ActivityPrefix();
                string statusText = StatusText(status);
                string suffix = (state != null) ? $", {state}" : string.Empty;
                this.events.Add($"{prefix}{nameof(ISocketEvents.ReceiveEnd)}({(int)id}, {result}, {statusText}{suffix})");
            }

            public string DisposeStartBase(SocketId id)
            {
                this.events.Add($"{nameof(ISocketEvents.DisposeStart)}({(int)id})");
                return "State_" + (int)id;
            }

            public void DisposeEndBase(SocketId id, string state)
            {
                string suffix = (state != null) ? $", {state}" : string.Empty;
                this.events.Add($"{nameof(ISocketEvents.DisposeEnd)}({(int)id}{suffix})");
            }
        }

        private sealed class StubSocketEvents : StubSocketEventsBase, ISocketEvents
        {
            public StubSocketEvents(IList<string> events)
                : base(events)
            {
            }

            public void SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint)
            {
                this.SendStartBase(id, bufferSize, endpoint);
            }

            public void SendEnd(SocketId id, OperationStatus status)
            {
                this.SendEndBase(id, status, null);
            }

            public void ReceiveStart(SocketId id, int bufferSize) => this.ReceiveStartBase(id, bufferSize);

            public void ReceiveEnd(SocketId id, int result, OperationStatus status)
            {
                this.ReceiveEndBase(id, result, status, null);
            }

            public void DisposeStart(SocketId id) => this.DisposeStartBase(id);

            public void DisposeEnd(SocketId id) => this.DisposeEndBase(id, null);
        }

        private sealed class StubSocketEventsState : StubSocketEventsBase, ISocketEvents<string>
        {
            public StubSocketEventsState(IList<string> events)
                : base(events)
            {
            }

            public string SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint)
            {
                return this.SendStartBase(id, bufferSize, endpoint);
            }

            public void SendEnd(SocketId id, OperationStatus status, string state)
            {
                this.SendEndBase(id, status, state);
            }

            public string ReceiveStart(SocketId id, int bufferSize) => this.ReceiveStartBase(id, bufferSize);

            public void ReceiveEnd(SocketId id, int result, OperationStatus status, string state)
            {
                this.ReceiveEndBase(id, result, status, state);
            }

            public string DisposeStart(SocketId id) => this.DisposeStartBase(id);

            public void DisposeEnd(SocketId id, string state) => this.DisposeEndBase(id, state);
        }
    }
}
