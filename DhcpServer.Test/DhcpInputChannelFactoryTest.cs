// <copyright file="DhcpInputChannelFactoryTest.cs" company="Brian Rogers">
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
    public sealed class DhcpInputChannelFactoryTest
    {
        [TestMethod]
        public void CreateAndReceiveFromTwo()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannelFactory factory = new DhcpInputChannelFactory(socket);

            IDhcpInputChannel channel1 = factory.CreateChannel(new Memory<byte>(new byte[500]));
            IDhcpInputChannel channel2 = factory.CreateChannel(new Memory<byte>(new byte[500]));
            Task<(DhcpMessageBuffer, DhcpError)> task1 = channel1.ReceiveAsync(CancellationToken.None);
            Task<(DhcpMessageBuffer, DhcpError)> task2 = channel2.ReceiveAsync(CancellationToken.None);

            task1.IsCompleted.Should().BeFalse();
            task2.IsCompleted.Should().BeFalse();

            socket.Complete("Request1");

            task1.IsCompleted.Should().BeTrue();
            task2.IsCompleted.Should().BeFalse();
            (DhcpMessageBuffer buffer1, DhcpError error1) = task1.Result;
            error1.Code.Should().Be(DhcpErrorCode.None);
            buffer1.Opcode.Should().Be(DhcpOpcode.Request);
            buffer1.TransactionId.Should().Be(0x00003D1D);

            socket.Complete(new DhcpException(DhcpErrorCode.SocketError, null));

            task2.IsCompleted.Should().BeTrue();
            (_, DhcpError error2) = task2.Result;
            error2.Code.Should().Be(DhcpErrorCode.SocketError);
        }
    }
}
