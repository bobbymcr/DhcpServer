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
        public void TooSmall()
        {
            Action act = () => new DhcpMessageBuffer(new Memory<byte>(new byte[239]));

            act.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("buffer");
        }

        [TestMethod]
        public void LoadRequestTooSmall()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            PacketResource.Read("Request1", buffer.Span);

            buffer.Load(239).Should().BeFalse();

            buffer.Opcode.Should().Be(DhcpOpcode.None);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.None);
            buffer.HardwareAddressLength.Should().Be(0);
            buffer.Hops.Should().Be(0);
            buffer.TransactionId.Should().Be(0);
            buffer.Seconds.Should().Be(0);
            buffer.Flags.Should().Be(DhcpFlags.None);
            buffer.ClientIPAddress.Should().Be(default(IPAddressV4));
            buffer.YourIPAddress.Should().Be(default(IPAddressV4));
            buffer.ServerIPAddress.Should().Be(default(IPAddressV4));
            buffer.GatewayIPAddress.Should().Be(default(IPAddressV4));
            HexString(buffer.ClientHardwareAddress).Should().BeEmpty();
            AsciiString(buffer.ServerHostName).Should().BeEmpty();
            AsciiString(buffer.BootFileName).Should().BeEmpty();
            buffer.MagicCookie.Should().Be(MagicCookie.None);
        }

        [TestMethod]
        public void LoadRequest()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = PacketResource.Read("Request1", buffer.Span);

            buffer.Load(length).Should().BeTrue();

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
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0x00, 0x0B, 0x82, 0x01, 0xFC, 0x42));
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

            buffer.Load(length).Should().BeTrue();

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
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0x00, 0x13, 0x20, 0x4E, 0x06, 0xD3));
            AsciiString(buffer.ServerHostName).Should().Be("MyHostName");
            AsciiString(buffer.BootFileName).Should().Be(@"Some\Boot\File.xyz");
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
        }

        [TestMethod]
        public void SaveAndLoad()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw))
            {
                Opcode = DhcpOpcode.Reply,
                HardwareAddressType = DhcpHardwareAddressType.Ethernet10Mb,
                HardwareAddressLength = 6,
                Hops = 1,
                TransactionId = 0x12345678,
                Seconds = 34,
                Flags = DhcpFlags.Broadcast,
                ClientIPAddress = new IPAddressV4(1, 2, 3, 4),
                YourIPAddress = new IPAddressV4(5, 6, 7, 8),
                ServerIPAddress = new IPAddressV4(9, 10, 11, 12),
                GatewayIPAddress = new IPAddressV4(13, 14, 15, 16),
                MagicCookie = MagicCookie.Dhcp,
            };
            output.ClientHardwareAddress[0] = 0xAA;
            output.ServerHostName[0] = (byte)'S';
            output.BootFileName[0] = (byte)'B';
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));

            output.Save();
            buffer.Load(500);

            buffer.Opcode.Should().Be(DhcpOpcode.Reply);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.Ethernet10Mb);
            buffer.HardwareAddressLength.Should().Be(6);
            buffer.Hops.Should().Be(1);
            buffer.TransactionId.Should().Be(0x12345678);
            buffer.Seconds.Should().Be(34);
            buffer.Flags.Should().Be(DhcpFlags.Broadcast);
            buffer.ClientIPAddress.Should().Be(new IPAddressV4(1, 2, 3, 4));
            buffer.YourIPAddress.Should().Be(new IPAddressV4(5, 6, 7, 8));
            buffer.ServerIPAddress.Should().Be(new IPAddressV4(9, 10, 11, 12));
            buffer.GatewayIPAddress.Should().Be(new IPAddressV4(13, 14, 15, 16));
            HexString(buffer.ClientHardwareAddress).Should().Be("AA0000000000");
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0xAA, 0x00, 0x00, 0x00, 0x00, 0x00));
            AsciiString(buffer.ServerHostName).Should().Be("S");
            AsciiString(buffer.BootFileName).Should().Be("B");
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
