// <copyright file="DhcpMessageBufferExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides extension methods for <see cref="DhcpMessageBuffer"/>.
    /// </summary>
    public static class DhcpMessageBufferExtensions
    {
        /// <summary>
        /// Writes data for the subnet mask option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="mask">The subnet mask.</param>
        public static void WriteSubnetMaskOption(this DhcpMessageBuffer buffer, IPAddressV4 mask)
        {
            WriteIPs(buffer, DhcpOptionTag.SubnetMask, mask);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.Router, ip1);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        /// <param name="ip2">The second router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.Router, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        /// <param name="ip2">The second router IP.</param>
        /// <param name="ip3">The third router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.Router, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        /// <param name="ip2">The second router IP.</param>
        /// <param name="ip3">The third router IP.</param>
        /// <param name="ip4">The fourth router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.Router, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.TimeServer, ip1);
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        /// <param name="ip2">The second time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.TimeServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        /// <param name="ip2">The second time server IP.</param>
        /// <param name="ip3">The third time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.TimeServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        /// <param name="ip2">The second time server IP.</param>
        /// <param name="ip3">The third time server IP.</param>
        /// <param name="ip4">The fourth time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.TimeServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the name server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first name server IP.</param>
        public static void WriteNameServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.NameServer, ip1);
        }

        /// <summary>
        /// Writes data for the name server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first name server IP.</param>
        /// <param name="ip2">The second name server IP.</param>
        public static void WriteNameServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.NameServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the name server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first name server IP.</param>
        /// <param name="ip2">The second name server IP.</param>
        /// <param name="ip3">The third name server IP.</param>
        public static void WriteNameServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.NameServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the name server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first name server IP.</param>
        /// <param name="ip2">The second name server IP.</param>
        /// <param name="ip3">The third name server IP.</param>
        /// <param name="ip4">The fourth name server IP.</param>
        public static void WriteNameServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.NameServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the domain server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first domain server IP.</param>
        public static void WriteDomainServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.DomainServer, ip1);
        }

        /// <summary>
        /// Writes data for the domain server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first domain server IP.</param>
        /// <param name="ip2">The second domain server IP.</param>
        public static void WriteDomainServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.DomainServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the domain server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first domain server IP.</param>
        /// <param name="ip2">The second domain server IP.</param>
        /// <param name="ip3">The third domain server IP.</param>
        public static void WriteDomainServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.DomainServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the domain server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first domain server IP.</param>
        /// <param name="ip2">The second domain server IP.</param>
        /// <param name="ip3">The third domain server IP.</param>
        /// <param name="ip4">The fourth domain server IP.</param>
        public static void WriteDomainServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.DomainServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the log server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first log server IP.</param>
        public static void WriteLogServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.LogServer, ip1);
        }

        /// <summary>
        /// Writes data for the log server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first log server IP.</param>
        /// <param name="ip2">The second log server IP.</param>
        public static void WriteLogServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.LogServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the log server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first log server IP.</param>
        /// <param name="ip2">The second log server IP.</param>
        /// <param name="ip3">The third log server IP.</param>
        public static void WriteLogServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.LogServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the log server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first log server IP.</param>
        /// <param name="ip2">The second log server IP.</param>
        /// <param name="ip3">The third log server IP.</param>
        /// <param name="ip4">The fourth log server IP.</param>
        public static void WriteLogServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.LogServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the quotes server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first quotes server IP.</param>
        public static void WriteQuotesServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.QuotesServer, ip1);
        }

        /// <summary>
        /// Writes data for the quotes server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first quotes server IP.</param>
        /// <param name="ip2">The second quotes server IP.</param>
        public static void WriteQuotesServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.QuotesServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the quotes server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first quotes server IP.</param>
        /// <param name="ip2">The second quotes server IP.</param>
        /// <param name="ip3">The third quotes server IP.</param>
        public static void WriteQuotesServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.QuotesServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the quotes server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first quotes server IP.</param>
        /// <param name="ip2">The second quotes server IP.</param>
        /// <param name="ip3">The third quotes server IP.</param>
        /// <param name="ip4">The fourth quotes server IP.</param>
        public static void WriteQuotesServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.QuotesServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the LPR server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first LPR server IP.</param>
        public static void WriteLprServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.LprServer, ip1);
        }

        /// <summary>
        /// Writes data for the LPR server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first LPR server IP.</param>
        /// <param name="ip2">The second LPR server IP.</param>
        public static void WriteLprServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.LprServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the LPR server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first LPR server IP.</param>
        /// <param name="ip2">The second LPR server IP.</param>
        /// <param name="ip3">The third LPR server IP.</param>
        public static void WriteLprServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.LprServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the LPR server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first LPR server IP.</param>
        /// <param name="ip2">The second LPR server IP.</param>
        /// <param name="ip3">The third LPR server IP.</param>
        /// <param name="ip4">The fourth LPR server IP.</param>
        public static void WriteLprServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.LprServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the Impress server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first Impress server IP.</param>
        public static void WriteImpressServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.ImpressServer, ip1);
        }

        /// <summary>
        /// Writes data for the Impress server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first Impress server IP.</param>
        /// <param name="ip2">The second Impress server IP.</param>
        public static void WriteImpressServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.ImpressServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the Impress server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first Impress server IP.</param>
        /// <param name="ip2">The second Impress server IP.</param>
        /// <param name="ip3">The third Impress server IP.</param>
        public static void WriteImpressServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.ImpressServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the Impress server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first Impress server IP.</param>
        /// <param name="ip2">The second Impress server IP.</param>
        /// <param name="ip3">The third Impress server IP.</param>
        /// <param name="ip4">The fourth Impress server IP.</param>
        public static void WriteImpressServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.ImpressServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the RLP server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first RLP server IP.</param>
        public static void WriteRlpServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.RlpServer, ip1);
        }

        /// <summary>
        /// Writes data for the RLP server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first RLP server IP.</param>
        /// <param name="ip2">The second RLP server IP.</param>
        public static void WriteRlpServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.RlpServer, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the RLP server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first RLP server IP.</param>
        /// <param name="ip2">The second RLP server IP.</param>
        /// <param name="ip3">The third RLP server IP.</param>
        public static void WriteRlpServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.RlpServer, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the RLP server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first RLP server IP.</param>
        /// <param name="ip2">The second RLP server IP.</param>
        /// <param name="ip3">The third RLP server IP.</param>
        /// <param name="ip4">The fourth RLP server IP.</param>
        public static void WriteRlpServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.RlpServer, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes UTF-8 encoded data for the host name option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="hostName">The host name buffer.</param>
        public static void WriteHostNameOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> hostName)
        {
            buffer.WriteOption(DhcpOptionTag.HostName, hostName, Encoding.UTF8);
        }

        /// <summary>
        /// Writes data for the boot file size option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="blocks">The size in 512-byte blocks.</param>
        public static void WriteBootFileSizeOption(this DhcpMessageBuffer buffer, ushort blocks)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.BootFileSize, 2);
            option.Data[0] = (byte)(blocks >> 8);
            option.Data[1] = (byte)(blocks & 0xFF);
        }

        /// <summary>
        /// Writes ASCII encoded data for the merit dump file option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="dumpFileName">The dump file name buffer.</param>
        public static void WriteMeritDumpFileOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> dumpFileName)
        {
            buffer.WriteOption(DhcpOptionTag.MeritDumpFile, dumpFileName, Encoding.ASCII);
        }

        /// <summary>
        /// Writes UTF-8 encoded data for the domain name option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="domainName">The domain name buffer.</param>
        public static void WriteDomainNameOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> domainName)
        {
            buffer.WriteOption(DhcpOptionTag.DomainName, domainName, Encoding.UTF8);
        }

        /// <summary>
        /// Writes data for the DHCP message type option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="messageType">The message type.</param>
        public static void WriteDhcpMsgTypeOption(this DhcpMessageBuffer buffer, DhcpMessageType messageType)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.DhcpMsgType, 1);
            option.Data[0] = (byte)messageType;
        }

        /// <summary>
        /// Writes data for the requested parameter list option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="p1">The first requested parameter.</param>
        public static void WriteParameterListOption(this DhcpMessageBuffer buffer, DhcpOptionTag p1)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ParameterList, 1);
            option.Data[0] = (byte)p1;
        }

        /// <summary>
        /// Writes data for the requested parameter list option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="p1">The first requested parameter.</param>
        /// <param name="p2">The second requested parameter.</param>
        public static void WriteParameterListOption(this DhcpMessageBuffer buffer, DhcpOptionTag p1, DhcpOptionTag p2)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ParameterList, 2);
            option.Data[0] = (byte)p1;
            option.Data[1] = (byte)p2;
        }

        /// <summary>
        /// Writes data for the requested parameter list option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="p1">The first requested parameter.</param>
        /// <param name="p2">The second requested parameter.</param>
        /// <param name="p3">The third requested parameter.</param>
        public static void WriteParameterListOption(this DhcpMessageBuffer buffer, DhcpOptionTag p1, DhcpOptionTag p2, DhcpOptionTag p3)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ParameterList, 3);
            option.Data[0] = (byte)p1;
            option.Data[1] = (byte)p2;
            option.Data[2] = (byte)p3;
        }

        /// <summary>
        /// Writes data for the requested parameter list option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="p1">The first requested parameter.</param>
        /// <param name="p2">The second requested parameter.</param>
        /// <param name="p3">The third requested parameter.</param>
        /// <param name="p4">The fourth requested parameter.</param>
        public static void WriteParameterListOption(this DhcpMessageBuffer buffer, DhcpOptionTag p1, DhcpOptionTag p2, DhcpOptionTag p3, DhcpOptionTag p4)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ParameterList, 4);
            option.Data[0] = (byte)p1;
            option.Data[1] = (byte)p2;
            option.Data[2] = (byte)p3;
            option.Data[3] = (byte)p4;
        }

        /// <summary>
        /// Writes the header for the relay agent information option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <returns>A buffer to allow writing relay agent sub-options.</returns>
        public static DhcpRelayAgentSubOptionsBuffer WriteRelayAgentInformationOptionHeader(this DhcpMessageBuffer buffer)
        {
            return new DhcpRelayAgentSubOptionsBuffer(buffer);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1)
        {
            var option = buffer.WriteOptionHeader(tag, 4);
            ip1.WriteTo(option.Data);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            var option = buffer.WriteOptionHeader(tag, 8);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            var option = buffer.WriteOptionHeader(tag, 12);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            var option = buffer.WriteOptionHeader(tag, 16);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
            ip4.WriteTo(option.Data.Slice(12));
        }
    }
}
