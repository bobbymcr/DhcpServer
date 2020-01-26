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

            public void Complete()
            {
                this.pending.SetResult(0);
            }

            public Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint)
            {
                this.pending = new TaskCompletionSource<int>();
                return this.pending.Task;
            }

            public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                throw new NotImplementedException();
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
        }
    }
}
