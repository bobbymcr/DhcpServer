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

            IDhcpInputChannel channel = outer.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeTrue();
            (DhcpMessageBuffer buffer, DhcpError error) = task.Result;
            buffer.Should().BeSameAs(inner.Buffer);
            error.Code.Should().Be(DhcpErrorCode.None);
            events.Should().ContainInOrder(
                "CreateChannelStart(1, 500)",
                "CreateChannelEnd(1, True, <null>)");
        }

        [TestMethod]
        public void WithEventsCreateChannelWithException()
        {
            List<string> events = new List<string>();
            StubInputChannelFactory inner = new StubInputChannelFactory();
            IDhcpInputChannelFactory outer = inner.WithEvents(new StubInputChannelFactoryEvents(events));

            Action act = () => outer.CreateChannel(new Memory<byte>(new byte[0]));

            act.Should().Throw<ArgumentOutOfRangeException>();
            events.Should().ContainInOrder(
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
            events.Should().ContainInOrder(
                "CreateChannelStart(1, 500)",
                "CreateChannelEnd(1, True, <null>)",
                "CreateChannelStart(2, 499)",
                "CreateChannelEnd(2, True, <null>)");
        }

        private sealed class StubInputChannelFactoryEvents : IDhcpInputChannelFactoryEvents
        {
            private readonly IList<string> events;

            public StubInputChannelFactoryEvents(IList<string> events)
            {
                this.events = events;
            }

            public void CreateChannelStart(int id, int bufferSize)
            {
                this.events.Add($"{nameof(this.CreateChannelStart)}({id}, {bufferSize})");
            }

            public void CreateChannelEnd(int id, bool succeeded, Exception exception)
            {
                string type = (exception != null) ? exception.GetType().Name : "<null>";
                this.events.Add($"{nameof(this.CreateChannelEnd)}({id}, {succeeded}, {type})");
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
