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
            OptionsString(buffer).Should().BeEmpty();
        }

        [TestMethod]
        public void LoadRequest()
        {
            const string ExpectedOptions =
@"DhcpMsgType={01}
ClientId={01000B8201FC42}
AddressRequest={00000000}
ParameterList={0103062A}
End={}
";
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
            buffer.ClientIPAddress.Should().Be(IP(1, 2, 3, 4));
            buffer.YourIPAddress.Should().Be(default(IPAddressV4));
            buffer.ServerIPAddress.Should().Be(default(IPAddressV4));
            buffer.GatewayIPAddress.Should().Be(default(IPAddressV4));
            HexString(buffer.ClientHardwareAddress).Should().Be("000B8201FC42");
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0x00, 0x0B, 0x82, 0x01, 0xFC, 0x42));
            AsciiString(buffer.ServerHostName).Should().BeEmpty();
            AsciiString(buffer.BootFileName).Should().BeEmpty();
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
            OptionsString(buffer).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void LoadOverload1()
        {
            const string ExpectedOptions =
@"DhcpMsgType={01}
DhcpMaxMsgSize={024E}
ParameterList={011C032B}
AddressTime={00000E10}
Overload={01}
DhcpMessage={50616464696E67}
ClientId={0100006C82DC4E}
End={}
DhcpMessage={66696C65206E616D65206669656C64206F7665726C6F6164}
End={}
";
            TestOverload("Overload1", ExpectedOptions);
        }

        [TestMethod]
        public void LoadOverload2()
        {
            const string ExpectedOptions =
@"DhcpMsgType={01}
DhcpMaxMsgSize={024E}
ParameterList={011C032B}
AddressTime={00000E10}
Overload={02}
DhcpMessage={50616464696E67}
ClientId={0100006C82DC4E}
End={}
DhcpMessage={736E616D65206669656C64206F7665726C6F6164}
End={}
";
            TestOverload("Overload2", ExpectedOptions);
        }

        [TestMethod]
        public void LoadOverload3()
        {
            const string ExpectedOptions =
@"DhcpMsgType={01}
DhcpMaxMsgSize={024E}
ParameterList={011C032B}
AddressTime={00000E10}
Overload={03}
DhcpMessage={50616464696E67}
ClientId={0100006C82DC4E}
End={}
DhcpMessage={66696C65206E616D65206669656C64206F7665726C6F6164}
End={}
DhcpMessage={736E616D65206669656C64206F7665726C6F6164}
End={}
";
            TestOverload("Overload3", ExpectedOptions);
        }

        [TestMethod]
        public void LoadReply()
        {
            const string ExpectedOptions =
@"DhcpMsgType={02}
SubnetMask={FFFFFF00}
Router={C0A80101}
AddressTime={00015180}
DhcpServerId={C0A80101}
DomainServer={09070A0F09070A1009070A12}
End={}
";
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
            buffer.YourIPAddress.Should().Be(IP(192, 168, 1, 100));
            buffer.ServerIPAddress.Should().Be(IP(192, 168, 1, 1));
            buffer.GatewayIPAddress.Should().Be(IP(153, 152, 151, 150));
            HexString(buffer.ClientHardwareAddress).Should().Be("0013204E06D3");
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0x00, 0x13, 0x20, 0x4E, 0x06, 0xD3));
            AsciiString(buffer.ServerHostName).Should().Be("MyHostName");
            AsciiString(buffer.BootFileName).Should().Be(@"Some\Boot\File.xyz");
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
            OptionsString(buffer).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void SaveAndLoad()
        {
            byte[] raw = new byte[256];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            TestSaveAndLoad(raw, output);
            TestSaveAndLoad(raw, output);
            TestSaveAndLoad(raw, output);
        }

        [TestMethod]
        public void Option1()
        {
            TestOption(
                o => o.WriteSubnetMaskOption(IP(1, 2, 3, 4)),
                "SubnetMask={01020304}");
        }

        [TestMethod]
        public void Option3()
        {
            TestOption(
                o => o.WriteRouterOption(IP(1, 2, 3, 4)),
                "Router={01020304}");
            TestOption(
                o => o.WriteRouterOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "Router={0102030405060708}");
            TestOption(
                o => o.WriteRouterOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "Router={010203040506070809080706}");
            TestOption(
                o => o.WriteRouterOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "Router={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option4()
        {
            TestOption(
                o => o.WriteTimeServerOption(IP(1, 2, 3, 4)),
                "TimeServer={01020304}");
            TestOption(
                o => o.WriteTimeServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "TimeServer={0102030405060708}");
            TestOption(
                o => o.WriteTimeServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "TimeServer={010203040506070809080706}");
            TestOption(
                o => o.WriteTimeServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "TimeServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option5()
        {
            TestOption(
                o => o.WriteNameServerOption(IP(1, 2, 3, 4)),
                "NameServer={01020304}");
            TestOption(
                o => o.WriteNameServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "NameServer={0102030405060708}");
            TestOption(
                o => o.WriteNameServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "NameServer={010203040506070809080706}");
            TestOption(
                o => o.WriteNameServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "NameServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option6()
        {
            TestOption(
                o => o.WriteDomainServerOption(IP(1, 2, 3, 4)),
                "DomainServer={01020304}");
            TestOption(
                o => o.WriteDomainServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "DomainServer={0102030405060708}");
            TestOption(
                o => o.WriteDomainServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "DomainServer={010203040506070809080706}");
            TestOption(
                o => o.WriteDomainServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "DomainServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option7()
        {
            TestOption(
                o => o.WriteLogServerOption(IP(1, 2, 3, 4)),
                "LogServer={01020304}");
            TestOption(
                o => o.WriteLogServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "LogServer={0102030405060708}");
            TestOption(
                o => o.WriteLogServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "LogServer={010203040506070809080706}");
            TestOption(
                o => o.WriteLogServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "LogServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option8()
        {
            TestOption(
                o => o.WriteQuotesServerOption(IP(1, 2, 3, 4)),
                "QuotesServer={01020304}");
            TestOption(
                o => o.WriteQuotesServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "QuotesServer={0102030405060708}");
            TestOption(
                o => o.WriteQuotesServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "QuotesServer={010203040506070809080706}");
            TestOption(
                o => o.WriteQuotesServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "QuotesServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option9()
        {
            TestOption(
                o => o.WriteLprServerOption(IP(1, 2, 3, 4)),
                "LprServer={01020304}");
            TestOption(
                o => o.WriteLprServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "LprServer={0102030405060708}");
            TestOption(
                o => o.WriteLprServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "LprServer={010203040506070809080706}");
            TestOption(
                o => o.WriteLprServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "LprServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option10()
        {
            TestOption(
                o => o.WriteImpressServerOption(IP(1, 2, 3, 4)),
                "ImpressServer={01020304}");
            TestOption(
                o => o.WriteImpressServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "ImpressServer={0102030405060708}");
            TestOption(
                o => o.WriteImpressServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "ImpressServer={010203040506070809080706}");
            TestOption(
                o => o.WriteImpressServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "ImpressServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option11()
        {
            TestOption(
                o => o.WriteRlpServerOption(IP(1, 2, 3, 4)),
                "RlpServer={01020304}");
            TestOption(
                o => o.WriteRlpServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "RlpServer={0102030405060708}");
            TestOption(
                o => o.WriteRlpServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "RlpServer={010203040506070809080706}");
            TestOption(
                o => o.WriteRlpServerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "RlpServer={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option12()
        {
            TestOption(
                o => o.WriteHostNameOption("MyHost"),
                "HostName={4D79486F7374}");
        }

        [TestMethod]
        public void Option13()
        {
            TestOption(
                o => o.WriteBootFileSizeOption(0x1234),
                "BootFileSize={1234}");
        }

        [TestMethod]
        public void Option14()
        {
            TestOption(
                o => o.WriteMeritDumpFileOption("file.dmp"),
                "MeritDumpFile={66696C652E646D70}");
        }

        [TestMethod]
        public void Option15()
        {
            TestOption(
                o => o.WriteDomainNameOption("MyName.com"),
                "DomainName={4D794E616D652E636F6D}");
        }

        private static void TestOption(Action<DhcpMessageBuffer> act, string expectedOption)
        {
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            act(output);
            output.WriteEndOption();

            OptionsString(output).Should().Be(
                expectedOption + Environment.NewLine +
                "End={}" + Environment.NewLine);
        }

        private static void TestOverload(string name, string expectedOptions)
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = PacketResource.Read(name, buffer.Span);

            buffer.Load(length).Should().BeTrue();

            buffer.Opcode.Should().Be(DhcpOpcode.Request);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.Ethernet10Mb);
            buffer.HardwareAddressLength.Should().Be(6);
            buffer.Hops.Should().Be(0);
            buffer.TransactionId.Should().Be(0xAC2EFFFF);
            buffer.Seconds.Should().Be(0);
            buffer.Flags.Should().Be(DhcpFlags.None);
            buffer.ClientIPAddress.Should().Be(default(IPAddressV4));
            buffer.YourIPAddress.Should().Be(default(IPAddressV4));
            buffer.ServerIPAddress.Should().Be(default(IPAddressV4));
            buffer.GatewayIPAddress.Should().Be(default(IPAddressV4));
            HexString(buffer.ClientHardwareAddress).Should().Be("00006C82DC4E");
            buffer.ServerHostName[0].Should().Be((byte)DhcpOptionTag.DhcpMessage);
            buffer.BootFileName[0].Should().Be((byte)DhcpOptionTag.DhcpMessage);
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
            OptionsString(buffer).Should().Be(expectedOptions);
        }

        private static void TestSaveAndLoad(byte[] raw, DhcpMessageBuffer output)
        {
            const string ExpectedOptions =
@"DhcpMsgType={02}
DhcpServerId={0A141E28}
End={}
";

            output.Opcode = DhcpOpcode.Reply;
            output.HardwareAddressType = DhcpHardwareAddressType.Ethernet10Mb;
            output.HardwareAddressLength = 6;
            output.Hops = 1;
            output.TransactionId = 0x12345678;
            output.Seconds = 34;
            output.Flags = DhcpFlags.Broadcast;
            output.ClientIPAddress = IP(1, 2, 3, 4);
            output.YourIPAddress = IP(5, 6, 7, 8);
            output.ServerIPAddress = IP(9, 10, 11, 12);
            output.GatewayIPAddress = IP(13, 14, 15, 16);
            output.MagicCookie = MagicCookie.Dhcp;
            output.ClientHardwareAddress[0] = 0xAA;
            output.ServerHostName[0] = (byte)'S';
            output.BootFileName[0] = (byte)'B';
            DhcpOption msgType = output.WriteOption(DhcpOptionTag.DhcpMsgType, 1);
            msgType.Data[0] = (byte)DhcpMessageType.Offer;
            DhcpOption serverId = output.WriteOption(DhcpOptionTag.DhcpServerId, 4);
            IP(10, 20, 30, 40).WriteTo(serverId.Data);
            output.WritePadding(5);
            output.WriteEndOption();

            int length = output.Save();
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            buffer.Load(length).Should().BeTrue();

            buffer.Opcode.Should().Be(DhcpOpcode.Reply);
            buffer.HardwareAddressType.Should().Be(DhcpHardwareAddressType.Ethernet10Mb);
            buffer.HardwareAddressLength.Should().Be(6);
            buffer.Hops.Should().Be(1);
            buffer.TransactionId.Should().Be(0x12345678);
            buffer.Seconds.Should().Be(34);
            buffer.Flags.Should().Be(DhcpFlags.Broadcast);
            buffer.ClientIPAddress.Should().Be(IP(1, 2, 3, 4));
            buffer.YourIPAddress.Should().Be(IP(5, 6, 7, 8));
            buffer.ServerIPAddress.Should().Be(IP(9, 10, 11, 12));
            buffer.GatewayIPAddress.Should().Be(IP(13, 14, 15, 16));
            HexString(buffer.ClientHardwareAddress).Should().Be("AA0000000000");
            new MacAddress(buffer.ClientHardwareAddress).Should().Be(new MacAddress(0xAA, 0x00, 0x00, 0x00, 0x00, 0x00));
            AsciiString(buffer.ServerHostName).Should().Be("S");
            AsciiString(buffer.BootFileName).Should().Be("B");
            buffer.MagicCookie.Should().Be(MagicCookie.Dhcp);
            OptionsString(buffer).Should().Be(ExpectedOptions);
        }

        private static IPAddressV4 IP(byte b1, byte b2, byte b3, byte b4) => new IPAddressV4(b1, b2, b3, b4);

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

        private static string OptionsString(DhcpMessageBuffer buffer)
        {
            StringBuilder sb = new StringBuilder();
            buffer.ReadOptions(sb, (o, s) => s.AppendLine(OptionString(o)));
            return sb.ToString();
        }

        private static string OptionString(DhcpOption option)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(option.Tag);
            sb.Append("={");
            sb.Append(HexString(option.Data));
            sb.Append("}");
            return sb.ToString();
        }
    }
}
