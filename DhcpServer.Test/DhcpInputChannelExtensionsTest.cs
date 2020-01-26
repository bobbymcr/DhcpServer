// <copyright file="DhcpInputChannelExtensionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
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
