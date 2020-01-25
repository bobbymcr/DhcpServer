// <copyright file="DhcpInputChannelTest.cs" company="Brian Rogers">
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
    public sealed class DhcpInputChannelTest
    {
        [TestMethod]
        public void Receive()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete("Request1");

            task.IsCompleted.Should().BeTrue();
            (DhcpMessageBuffer buffer, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.None);
            buffer.Opcode.Should().Be(DhcpOpcode.Request);
            buffer.TransactionId.Should().Be(0x00003D1D);
        }

        [TestMethod]
        public void CancelReceive()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            using CancellationTokenSource cts = new CancellationTokenSource();

            cts.Cancel();
            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(cts.Token);

            task.IsCanceled.Should().BeTrue();
        }

        [TestMethod]
        public void PacketTooSmall()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete("Request1", 50);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooSmall);
        }

        [TestMethod]
        public void PacketTooLarge()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete("LargeRequest1", 65536);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooLarge);
        }

        [TestMethod]
        public void PacketTooLargeForBuffer()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete("LargeRequest1", 600);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.PacketTooLarge);
        }

        [TestMethod]
        public void BufferTooSmall()
        {
            Memory<byte> rawBuffer = new Memory<byte>(new byte[50]);

            Action act = () => new DhcpInputChannel(new StubInputSocket(), rawBuffer);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ReceiveWithSocketError()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            Exception inner = new Exception();
            Exception exception = new DhcpException(DhcpErrorCode.SocketError, inner);

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete(exception);

            task.IsCompleted.Should().BeTrue();
            (_, DhcpError error) = task.Result;
            error.Code.Should().Be(DhcpErrorCode.SocketError);
            error.Exception.Should().BeSameAs(exception);
            error.Exception.InnerException.Should().BeSameAs(inner);
        }

        [TestMethod]
        public void ReceiveWithUnhandledException()
        {
            StubInputSocket socket = new StubInputSocket();
            DhcpInputChannel channel = new DhcpInputChannel(socket, new Memory<byte>(new byte[500]));
            Exception exception = new InvalidOperationException("won't be handled");

            Task<ValueTuple<DhcpMessageBuffer, DhcpError>> task = channel.ReceiveAsync(CancellationToken.None);

            task.IsCompleted.Should().BeFalse();

            socket.Complete(exception);

            task.IsFaulted.Should().BeTrue();
            task.Exception.Should().NotBeNull();
            task.Exception.InnerExceptions.Should().ContainSingle().Which.Should().BeSameAs(exception);
        }
    }
}
