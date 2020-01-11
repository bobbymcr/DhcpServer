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
        public void TooLongHardwareAddressLength()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));

            buffer.HardwareAddressLength = 17;

            buffer.ClientHardwareAddress.Length.Should().Be(16);
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
            buffer.Length.Should().Be(0);
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

            buffer.Length.Should().Be(length);
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

            buffer.Length.Should().Be(length);
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

        [TestMethod]
        public void Option16()
        {
            TestOption(
                o => o.WriteSwapServerOption(IP(0x1, 0x2, 0x3, 0x4)),
                "SwapServer={01020304}");
        }

        [TestMethod]
        public void Option17()
        {
            TestOption(
                o => o.WriteRootPathOption(@"\x\y\z"),
                "RootPath={5C785C795C7A}");
        }

        [TestMethod]
        public void Option18()
        {
            TestOption(
                o => o.WriteExtensionFileOption(@"\x\y\z"),
                "ExtensionFile={5C785C795C7A}");
        }

        [TestMethod]
        public void Option19()
        {
            TestOption(
                o => o.WriteForwardOption(true),
                "Forward={01}");
        }

        [TestMethod]
        public void Option20()
        {
            TestOption(
                o => o.WriteSrcRteOption(false),
                "SrcRte={00}");
        }

        [TestMethod]
        public void Option21()
        {
            TestOption(
                o => o.WritePolicyFilterOption(IP(1, 2, 3, 0), IP(0, 0, 0, 255)),
                "PolicyFilter={01020300000000FF}");
            TestOption(
                o => o.WritePolicyFilterOption(IP(1, 2, 3, 0), IP(0, 0, 0, 255), IP(0, 8, 7, 6), IP(255, 0, 0, 0)),
                "PolicyFilter={01020300000000FF00080706FF000000}");
        }

        [TestMethod]
        public void Option22()
        {
            TestOption(
                o => o.WriteMaxDGAssemblyOption(0x9988),
                "MaxDGAssembly={9988}");
        }

        [TestMethod]
        public void Option23()
        {
            TestOption(
                o => o.WriteDefaultIPTtlOption(0x54),
                "DefaultIPTtl={54}");
        }

        [TestMethod]
        public void Option24()
        {
            TestOption(
                o => o.WriteMtuTimeoutOption(0x49813609),
                "MtuTimeout={49813609}");
        }

        [TestMethod]
        public void Option25()
        {
            TestOption(
                o => o.WriteMtuPlateauOption(0x5001),
                "MtuPlateau={5001}");
            TestOption(
                o => o.WriteMtuPlateauOption(0x5001, 0x6002),
                "MtuPlateau={50016002}");
            TestOption(
                o => o.WriteMtuPlateauOption(0x5001, 0x6002, 0x7003),
                "MtuPlateau={500160027003}");
            TestOption(
                o => o.WriteMtuPlateauOption(0x5001, 0x6002, 0x7003, 0x8004),
                "MtuPlateau={5001600270038004}");
        }

        [TestMethod]
        public void Option26()
        {
            TestOption(
                o => o.WriteMtuInterfaceOption(0x4433),
                "MtuInterface={4433}");
        }

        [TestMethod]
        public void Option27()
        {
            TestOption(
                o => o.WriteMtuSubnetOption(false),
                "MtuSubnet={00}");
        }

        [TestMethod]
        public void Option28()
        {
            TestOption(
                o => o.WriteBroadcastAddressOption(IP(0x7, 0x6, 0x5, 0xFF)),
                "BroadcastAddress={070605FF}");
        }

        [TestMethod]
        public void Option29()
        {
            TestOption(
                o => o.WriteMaskDiscoveryOption(true),
                "MaskDiscovery={01}");
        }

        [TestMethod]
        public void Option30()
        {
            TestOption(
                o => o.WriteMaskSupplierOption(false),
                "MaskSupplier={00}");
        }

        [TestMethod]
        public void Option31()
        {
            TestOption(
                o => o.WriteRouterDiscoveryOption(true),
                "RouterDiscovery={01}");
        }

        [TestMethod]
        public void Option32()
        {
            TestOption(
                o => o.WriteRouterRequestOption(IP(1, 2, 3, 4)),
                "RouterRequest={01020304}");
        }

        [TestMethod]
        public void Option33()
        {
            TestOption(
                o => o.WriteStaticRouteOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "StaticRoute={0102030405060708}");
            TestOption(
                o => o.WriteStaticRouteOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "StaticRoute={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option34()
        {
            TestOption(
                o => o.WriteTrailersOption(true),
                "Trailers={01}");
        }

        [TestMethod]
        public void Option35()
        {
            TestOption(
                o => o.WriteArpTimeoutOption(0x76543210),
                "ArpTimeout={76543210}");
        }

        [TestMethod]
        public void Option36()
        {
            TestOption(
                o => o.WriteEthernetOption(false),
                "Ethernet={00}");
        }

        [TestMethod]
        public void Option37()
        {
            TestOption(
                o => o.WriteDefaultTcpTtlOption(0x63),
                "DefaultTcpTtl={63}");
        }

        [TestMethod]
        public void Option38()
        {
            TestOption(
                o => o.WriteKeepaliveTimeOption(0x11223344),
                "KeepaliveTime={11223344}");
        }

        [TestMethod]
        public void Option39()
        {
            TestOption(
                o => o.WriteKeepaliveDataOption(true),
                "KeepaliveData={01}");
        }

        [TestMethod]
        public void Option40()
        {
            TestOption(
                o => o.WriteNisDomainOption("hello.net"),
                "NisDomain={68656C6C6F2E6E6574}");
        }

        [TestMethod]
        public void Option41()
        {
            TestOption(
                o => o.WriteNisServersOption(IP(1, 2, 3, 4)),
                "NisServers={01020304}");
            TestOption(
                o => o.WriteNisServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "NisServers={0102030405060708}");
            TestOption(
                o => o.WriteNisServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "NisServers={010203040506070809080706}");
            TestOption(
                o => o.WriteNisServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "NisServers={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option42()
        {
            TestOption(
                o => o.WriteNtpServersOption(IP(1, 2, 3, 4)),
                "NtpServers={01020304}");
            TestOption(
                o => o.WriteNtpServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "NtpServers={0102030405060708}");
            TestOption(
                o => o.WriteNtpServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "NtpServers={010203040506070809080706}");
            TestOption(
                o => o.WriteNtpServersOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "NtpServers={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option43WithSubOptions()
        {
            const string ExpectedOptions =
@"VendorSpecific={1203020406550177}
SubnetMask={FFFFFF00}
End={}
";
            const string ExpectedVendorSpecificOptions =
@"18={020406}
85={77}
";
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            DhcpSubOptionsBuffer buffer = output.WriteVendorSpecificOptionHeader();
            ReadOnlySpan<byte> data1 = new ReadOnlySpan<byte>(new byte[] { 0x2, 0x4, 0x6 });
            buffer.WriteDataItem(0x12, data1);
            ReadOnlySpan<byte> data2 = new ReadOnlySpan<byte>(new byte[] { 0x77 });
            buffer.WriteDataItem(0x55, data2);
            buffer.End();
            output.WriteSubnetMaskOption(IP(255, 255, 255, 0));
            output.WriteEndOption();

            SubOptionsString(output, (byte)DhcpOptionTag.VendorSpecific).Should().Be(ExpectedVendorSpecificOptions);
            OptionsString(output).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void Option43WithSubOptionsAndRawData()
        {
            const string ExpectedOptions =
@"VendorSpecific={1203020406550177FF09080706}
SubnetMask={FFFFFF00}
End={}
";
            const string ExpectedVendorSpecificOptions =
@"18={020406}
85={77}
255={09080706}
";
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            DhcpSubOptionsBuffer buffer = output.WriteVendorSpecificOptionHeader();
            ReadOnlySpan<byte> data1 = new ReadOnlySpan<byte>(new byte[] { 0x2, 0x4, 0x6 });
            buffer.WriteDataItem(0x12, data1);
            ReadOnlySpan<byte> data2 = new ReadOnlySpan<byte>(new byte[] { 0x77 });
            buffer.WriteDataItem(0x55, data2);
            ReadOnlySpan<byte> rawData = new ReadOnlySpan<byte>(new byte[] { 0x9, 0x8, 0x7, 0x6 });
            buffer.End(rawData);
            output.WriteSubnetMaskOption(IP(255, 255, 255, 0));
            output.WriteEndOption();

            SubOptionsString(output, (byte)DhcpOptionTag.VendorSpecific).Should().Be(ExpectedVendorSpecificOptions);
            OptionsString(output).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void Option43WithRawData()
        {
            const string ExpectedOptions =
@"VendorSpecific={0908070605}
SubnetMask={FFFFFF00}
End={}
";
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            ReadOnlySpan<byte> rawData = new ReadOnlySpan<byte>(new byte[] { 0x9, 0x8, 0x7, 0x6, 0x5 });
            output.WriteVendorSpecificOption(rawData);
            output.WriteSubnetMaskOption(IP(255, 255, 255, 0));
            output.WriteEndOption();

            OptionsString(output).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void Option44()
        {
            TestOption(
                o => o.WriteNetBiosNameSrvOption(IP(1, 2, 3, 4)),
                "NetBiosNameSrv={01020304}");
            TestOption(
                o => o.WriteNetBiosNameSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "NetBiosNameSrv={0102030405060708}");
            TestOption(
                o => o.WriteNetBiosNameSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "NetBiosNameSrv={010203040506070809080706}");
            TestOption(
                o => o.WriteNetBiosNameSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "NetBiosNameSrv={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option45()
        {
            TestOption(
                o => o.WriteNetBiosDistSrvOption(IP(1, 2, 3, 4)),
                "NetBiosDistSrv={01020304}");
            TestOption(
                o => o.WriteNetBiosDistSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "NetBiosDistSrv={0102030405060708}");
            TestOption(
                o => o.WriteNetBiosDistSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "NetBiosDistSrv={010203040506070809080706}");
            TestOption(
                o => o.WriteNetBiosDistSrvOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "NetBiosDistSrv={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option46()
        {
            TestOption46("NetBiosNodeType={00}", NetBiosNodeType.None);
            TestOption46("NetBiosNodeType={01}", NetBiosNodeType.Broadcast);
            TestOption46("NetBiosNodeType={02}", NetBiosNodeType.PointToPoint);
            TestOption46("NetBiosNodeType={04}", NetBiosNodeType.MixedMode);
            TestOption46("NetBiosNodeType={08}", NetBiosNodeType.Hybrid);
        }

        [TestMethod]
        public void Option47()
        {
            TestOption(
                o => o.WriteNetBiosScopeOption("Name.com"),
                "NetBiosScope={4E616D652E636F6D}");
        }

        [TestMethod]
        public void Option48()
        {
            TestOption(
                o => o.WriteXWindowFontOption(IP(1, 2, 3, 4)),
                "XWindowFont={01020304}");
            TestOption(
                o => o.WriteXWindowFontOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "XWindowFont={0102030405060708}");
            TestOption(
                o => o.WriteXWindowFontOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "XWindowFont={010203040506070809080706}");
            TestOption(
                o => o.WriteXWindowFontOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "XWindowFont={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option49()
        {
            TestOption(
                o => o.WriteXWindowManagerOption(IP(1, 2, 3, 4)),
                "XWindowManager={01020304}");
            TestOption(
                o => o.WriteXWindowManagerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8)),
                "XWindowManager={0102030405060708}");
            TestOption(
                o => o.WriteXWindowManagerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6)),
                "XWindowManager={010203040506070809080706}");
            TestOption(
                o => o.WriteXWindowManagerOption(IP(1, 2, 3, 4), IP(5, 6, 7, 8), IP(9, 8, 7, 6), IP(5, 4, 3, 2)),
                "XWindowManager={01020304050607080908070605040302}");
        }

        [TestMethod]
        public void Option50()
        {
            TestOption(
                o => o.WriteAddressRequestOption(IP(1, 2, 3, 4)),
                "AddressRequest={01020304}");
        }

        [TestMethod]
        public void Option51()
        {
            TestOption(
                o => o.WriteAddressTimeOption(0x55443322),
                "AddressTime={55443322}");
        }

        [TestMethod]
        public void Option53()
        {
            TestOption53("DhcpMsgType={00}", DhcpMessageType.None);
            TestOption53("DhcpMsgType={01}", DhcpMessageType.Discover);
            TestOption53("DhcpMsgType={02}", DhcpMessageType.Offer);
            TestOption53("DhcpMsgType={03}", DhcpMessageType.Request);
            TestOption53("DhcpMsgType={04}", DhcpMessageType.Decline);
            TestOption53("DhcpMsgType={05}", DhcpMessageType.Ack);
            TestOption53("DhcpMsgType={06}", DhcpMessageType.Nak);
            TestOption53("DhcpMsgType={07}", DhcpMessageType.Release);
            TestOption53("DhcpMsgType={08}", DhcpMessageType.Inform);
            TestOption53("DhcpMsgType={09}", DhcpMessageType.ForceRenew);
            TestOption53("DhcpMsgType={0A}", DhcpMessageType.LeaseQuery);
            TestOption53("DhcpMsgType={0B}", DhcpMessageType.LeaseUnassigned);
            TestOption53("DhcpMsgType={0C}", DhcpMessageType.LeaseUnknown);
            TestOption53("DhcpMsgType={0D}", DhcpMessageType.LeaseActive);
            TestOption53("DhcpMsgType={0E}", DhcpMessageType.BulkLeaseQuery);
            TestOption53("DhcpMsgType={0F}", DhcpMessageType.LeaseQueryDone);
            TestOption53("DhcpMsgType={10}", DhcpMessageType.ActiveLeaseQuery);
            TestOption53("DhcpMsgType={11}", DhcpMessageType.LeaseQueryStatus);
            TestOption53("DhcpMsgType={12}", DhcpMessageType.Tls);
        }

        [TestMethod]
        public void Option54()
        {
            TestOption(
                o => o.WriteDhcpServerIdOption(IP(1, 2, 3, 4)),
                "DhcpServerId={01020304}");
        }

        [TestMethod]
        public void Option55()
        {
            TestOption(
                o => o.WriteParameterListOption(DhcpOptionTag.SubnetMask),
                "ParameterList={01}");
            TestOption(
                o => o.WriteParameterListOption(DhcpOptionTag.SubnetMask, DhcpOptionTag.Router),
                "ParameterList={0103}");
            TestOption(
                o => o.WriteParameterListOption(DhcpOptionTag.SubnetMask, DhcpOptionTag.Router, DhcpOptionTag.TimeServer),
                "ParameterList={010304}");
            TestOption(
                o => o.WriteParameterListOption(DhcpOptionTag.SubnetMask, DhcpOptionTag.Router, DhcpOptionTag.TimeServer, DhcpOptionTag.NameServer),
                "ParameterList={01030405}");
        }

        [TestMethod]
        public void Option56()
        {
            TestOption(
                o => o.WriteDhcpMessageOption("error!"),
                "DhcpMessage={6572726F7221}");
        }

        [TestMethod]
        public void Option57()
        {
            TestOption(
                o => o.WriteDhcpMaxMsgSizeOption(0x4321),
                "DhcpMaxMsgSize={4321}");
        }

        [TestMethod]
        public void Option58()
        {
            TestOption(
                o => o.WriteRenewalTimeOption(0x23456789),
                "RenewalTime={23456789}");
        }

        [TestMethod]
        public void Option59()
        {
            TestOption(
                o => o.WriteRebindingTimeOption(0x11449916),
                "RebindingTime={11449916}");
        }

        [TestMethod]
        public void Option60()
        {
            TestOption(
                o => o.WriteClassIdOption("MyClass"),
                "ClassId={4D79436C617373}");
        }

        [TestMethod]
        public void Option82()
        {
            const string ExpectedOptions =
@"RelayAgentInformation={01056369726331020372653105040102030406027331}
SubnetMask={FFFFFF00}
End={}
";
            const string ExpectedRelayAgentOptions =
@"AgentCircuitId={6369726331}
AgentRemoteId={726531}
LinkSelection={01020304}
SubscriberId={7331}
";
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            DhcpRelayAgentSubOptionsBuffer buffer = output.WriteRelayAgentInformationOptionHeader();
            buffer.WriteAgentCircuitId("circ1");
            buffer.WriteAgentRemoteId("re1");
            buffer.WriteLinkSelection(IP(1, 2, 3, 4));
            buffer.WriteSubscriberId("s1");
            buffer.End();
            output.WriteSubnetMaskOption(IP(255, 255, 255, 0));
            output.WriteEndOption();

            RelayAgentOptionsString(output).Should().Be(ExpectedRelayAgentOptions);
            OptionsString(output).Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void WriteMultipleContainerOptions()
        {
            const string ExpectedOptions =
@"254={01020304}
RelayAgentInformation={01056369726331}
End={}
";
            const string ExpectedRelayAgentOptions =
@"AgentCircuitId={6369726331}
";
            const string ExpectedCustomOptions =
@"1={0304}
";
            byte[] raw = new byte[300];
            DhcpMessageBuffer output = new DhcpMessageBuffer(new Memory<byte>(raw));

            output.WriteContainerOptionHeader((DhcpOptionTag)254);
            DhcpSubOption sub1 = output.WriteSubOptionHeader(1, 2);
            sub1.Data[0] = 3;
            sub1.Data[1] = 4;
            output.EndContainerOption();
            DhcpRelayAgentSubOptionsBuffer buffer = output.WriteRelayAgentInformationOptionHeader();
            buffer.WriteAgentCircuitId("circ1");
            buffer.End();
            output.WriteEndOption();

            RelayAgentOptionsString(output).Should().Be(ExpectedRelayAgentOptions);
            SubOptionsString(output, 254).Should().Be(ExpectedCustomOptions);
            OptionsString(output).Should().Be(ExpectedOptions);
        }

        private static void TestOption46(string expectedOption, NetBiosNodeType nodeType)
        {
            TestOption(o => o.WriteNetBiosNodeTypeOption(nodeType), expectedOption);
        }

        private static void TestOption53(string expectedOption, DhcpMessageType messageType)
        {
            TestOption(o => o.WriteDhcpMsgTypeOption(messageType), expectedOption);
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
            DhcpOption msgType = output.WriteOptionHeader(DhcpOptionTag.DhcpMsgType, 1);
            msgType.Data[0] = (byte)DhcpMessageType.Offer;
            DhcpOption serverId = output.WriteOptionHeader(DhcpOptionTag.DhcpServerId, 4);
            IP(10, 20, 30, 40).WriteTo(serverId.Data);
            output.WritePadding(5);
            output.WriteEndOption();

            int length = output.Save();
            output.Length.Should().Be(length);
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
            foreach (DhcpOption option in buffer.Options)
            {
                sb.Append(option.Tag);
                sb.Append("={");
                sb.Append(HexString(option.Data));
                sb.AppendLine("}");
            }

            return sb.ToString();
        }

        private static string SubOptionsString(DhcpMessageBuffer buffer, byte tag)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DhcpOption option in buffer.Options)
            {
                if ((byte)option.Tag == tag)
                {
                    foreach (DhcpSubOption subOption in option)
                    {
                        sb.Append(subOption.Code);
                        sb.Append("={");
                        sb.Append(HexString(subOption.Data));
                        sb.AppendLine("}");
                    }
                }
            }

            return sb.ToString();
        }

        private static string RelayAgentOptionsString(DhcpMessageBuffer buffer)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DhcpOption option in buffer.Options)
            {
                if (option.Tag == DhcpOptionTag.RelayAgentInformation)
                {
                    foreach (DhcpSubOption subOption in option)
                    {
                        sb.Append((DhcpRelayAgentSubOptionCode)subOption.Code);
                        sb.Append("={");
                        sb.Append(HexString(subOption.Data));
                        sb.AppendLine("}");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
