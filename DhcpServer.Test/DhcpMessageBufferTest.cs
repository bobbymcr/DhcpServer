// <copyright file="DhcpMessageBufferTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Text;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DhcpMessageBufferTest
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
            buffer.ClientIPAddress.Should().Be(new IPAddressV4(1, 2, 3, 4));
            buffer.YourIPAddress.Should().Be(default(IPAddressV4));
            buffer.ServerIPAddress.Should().Be(default(IPAddressV4));
            buffer.GatewayIPAddress.Should().Be(default(IPAddressV4));
            HexString(buffer.ClientHardwareAddress).Should().Be("000B8201FC42");
            AsciiString(buffer.ServerHostName).Should().BeEmpty();
            AsciiString(buffer.BootFileName).Should().BeEmpty();
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
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
            buffer.ClientIPAddress.Should().Be(default(IPAddressV4));
            buffer.YourIPAddress.Should().Be(new IPAddressV4(192, 168, 1, 100));
            buffer.ServerIPAddress.Should().Be(new IPAddressV4(192, 168, 1, 1));
            buffer.GatewayIPAddress.Should().Be(new IPAddressV4(153, 152, 151, 150));
            HexString(buffer.ClientHardwareAddress).Should().Be("0013204E06D3");
            AsciiString(buffer.ServerHostName).Should().Be("MyHostName");
            AsciiString(buffer.BootFileName).Should().Be(@"Some\Boot\File.xyz");
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
        }

        private static string HexString(Span<byte> span)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in span)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static string AsciiString(Span<byte> span)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in span)
            {
                if (b == 0)
                {
                    break;
                }

                sb.Append((char)b);
            }

            return sb.ToString();
        }
    }
}
