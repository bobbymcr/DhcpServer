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
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = inner.WithEvents(1, new StubSocketEvents(events));
            IPEndpointV4 endpoint = new IPEndpointV4(new IPAddressV4(1, 2, 3, 4), 5566);
            ReadOnlyMemory<byte> buffer = new ReadOnlyMemory<byte>(new byte[500]);

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000011110000"));
            Task task = outer.SendAsync(buffer, endpoint);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            inner.Complete();

            task.IsCompleted.Should().BeTrue();
            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000011110000/SendStart(1, 500, 0x04030201, 5566)",
                "12345678-1234-5678-9abc-000011110000/SendEnd(1, True, <null>)");
        }

        [TestMethod]
        public void WithEventsSendException()
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = inner.WithEvents(2, new StubSocketEvents(events));
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
            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000022220000/SendStart(2, 499, 0x99887766, 443)",
                "12345678-1234-5678-9abc-000022220000/SendEnd(2, False, DhcpException)");
        }

        [TestMethod]
        public void WithEventsReceive()
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = inner.WithEvents(1, new StubSocketEvents(events));
            Memory<byte> buffer = new Memory<byte>(new byte[500]);

            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000033330000"));
            ValueTask<int> task = outer.ReceiveAsync(buffer, CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            inner.Complete(77);

            task.IsCompleted.Should().BeTrue();
            task.Result.Should().Be(77);
            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000033330000/ReceiveStart(1, 500)",
                "12345678-1234-5678-9abc-000033330000/ReceiveEnd(1, 77, True, <null>)");
        }

        [TestMethod]
        public void WithEventsReceiveException()
        {
            List<string> events = new List<string>();
            StubSocket inner = new StubSocket();
            ISocket outer = inner.WithEvents(2, new StubSocketEvents(events));
            Memory<byte> buffer = new Memory<byte>(new byte[499]);

            using CancellationTokenSource cts = new CancellationTokenSource();
            using ActivityScope scope1 = new ActivityScope(new Guid("12345678-1234-5678-9abc-000044440000"));
            ValueTask<int> task = outer.ReceiveAsync(buffer, cts.Token);

            task.IsCompleted.Should().BeFalse();

            using ActivityScope scope2 = new ActivityScope(new Guid("12345678-1234-5678-9abc-999999999999"));
            cts.Cancel();

            task.IsCanceled.Should().BeTrue();
            events.Should().HaveCount(2).And.ContainInOrder(
                "12345678-1234-5678-9abc-000044440000/ReceiveStart(2, 499)",
                "12345678-1234-5678-9abc-000044440000/ReceiveEnd(2, -1, False, TaskCanceledException)");
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

        private sealed class StubSocket : ISocket
        {
            private TaskCompletionSource<int> pending;

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

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class StubSocketEvents : ISocketEvents
        {
            private readonly IList<string> events;

            public StubSocketEvents(IList<string> events)
            {
                this.events = events;
            }

            public void SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.SendStart)}({(int)id}, {bufferSize}, 0x{((uint)endpoint.Address).ToString("X8")}, {endpoint.Port})");
            }

            public void SendEnd(SocketId id, bool succeeded, Exception exception)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.SendEnd)}({(int)id}, {succeeded}, {type})");
            }

            public void ReceiveStart(SocketId id, int bufferSize)
            {
                string prefix = ActivityPrefix();
                this.events.Add($"{prefix}{nameof(this.ReceiveStart)}({(int)id}, {bufferSize})");
            }

            public void ReceiveEnd(SocketId id, int result, bool succeeded, Exception exception)
            {
                string prefix = ActivityPrefix();
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{prefix}{nameof(this.ReceiveEnd)}({(int)id}, {result}, {succeeded}, {type})");
            }
        }
    }
}
