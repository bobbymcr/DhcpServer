// <copyright file="DhcpInputChannelFactoryExtensionsTest.cs" company="Brian Rogers">
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
    public sealed class DhcpInputChannelFactoryExtensionsTest
    {
        [TestMethod]
        public void WithEventsCreateChannel()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(new StubInputChannelFactoryEvents(events));
            using CancellationTokenSource cts = new CancellationTokenSource();

            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            (DhcpMessageBuffer buffer, DhcpError error) = task.Result;
            buffer.Should().BeSameAs(inner.Buffer);
            error.Code.Should().Be(DhcpErrorCode.None);
            events.Should().ContainInOrder(
                "CreateChannelStart(500)",
                "CreateChannelEnd(True, <null>)");
        }

        [TestMethod]
        public void WithEventsCreateChannelWithException()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(new StubInputChannelFactoryEvents(events));
            using CancellationTokenSource cts = new CancellationTokenSource();

            Action act = () => outer.CreateChannel(new Memory<byte>(new byte[0]));

            act.Should().Throw<ArgumentOutOfRangeException>();
            events.Should().ContainInOrder(
                "CreateChannelStart(0)",
                "CreateChannelEnd(False, ArgumentOutOfRangeException)");
        }

        private sealed class StubInputChannelFactoryEvents : IDhcpInputChannelFactoryEvents
        {
            private readonly IList<string> events;

            public StubInputChannelFactoryEvents(IList<string> events)
            {
                this.events = events;
            }

            public void CreateChannelStart(int bufferSize)
            {
                this.events.Add($"{nameof(this.CreateChannelStart)}({bufferSize})");
            }

            public void CreateChannelEnd(bool succeeded, Exception exception)
            {
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{nameof(this.CreateChannelEnd)}({succeeded}, {type})");
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
                    return Task.FromResult((this.buffer, this.error));
                }
            }
        }
    }
}
