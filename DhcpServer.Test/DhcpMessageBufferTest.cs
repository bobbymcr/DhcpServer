// <copyright file="DhcpMessageBufferTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DhcpMessageBufferTest
    {
        [TestMethod]
        public void LoadRequest()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = PacketResource.Read("Request1", buffer.Span);

            buffer.Load(length);

            buffer.Opcode.Should().Be(DhcpOpcode.Request);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.Ethernet10Mb);
            buffer.HardwareAddressLength.Should().Be(6);
            buffer.Hops.Should().Be(0);
            buffer.TransactionId.Should().Be(0x00003D1D);
            buffer.Seconds.Should().Be(258);
            buffer.Flags.Should().Be(DhcpFlags.Broadcast);
        }

        [TestMethod]
        public void LoadReply()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = PacketResource.Read("Reply1", buffer.Span);

            buffer.Load(length);

            buffer.Opcode.Should().Be(DhcpOpcode.Reply);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.Ethernet10Mb);
            buffer.HardwareAddressLength.Should().Be(6);
            buffer.Hops.Should().Be(3);
            buffer.TransactionId.Should().Be(0x3903F326);
            buffer.Seconds.Should().Be(0);
            buffer.Flags.Should().Be(DhcpFlags.None);
        }
    }
}
