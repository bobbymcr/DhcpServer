﻿// <copyright file="DhcpMessageBufferExtensions.cs" company="Brian Rogers">
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
            WriteUInt16(buffer, DhcpOptionTag.BootFileSize, blocks);
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
        /// Writes data for the swap server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip">The server address.</param>
        public static void WriteSwapServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip)
        {
            WriteIPs(buffer, DhcpOptionTag.SwapServer, ip);
        }

        /// <summary>
        /// Writes ASCII encoded data for the root path option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="rootPath">The root path buffer.</param>
        public static void WriteRootPathOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> rootPath)
        {
            buffer.WriteOption(DhcpOptionTag.RootPath, rootPath, Encoding.ASCII);
        }

        /// <summary>
        /// Writes UTF-8 encoded data for the extension file option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="extensionFile">The extension file buffer.</param>
        public static void WriteExtensionFileOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> extensionFile)
        {
            buffer.WriteOption(DhcpOptionTag.ExtensionFile, extensionFile, Encoding.UTF8);
        }

        /// <summary>
        /// Writes data for the forward option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="enable">The enable flag.</param>
        public static void WriteForwardOption(this DhcpMessageBuffer buffer, bool enable)
        {
            WriteFlag(buffer, DhcpOptionTag.Forward, enable);
        }

        /// <summary>
        /// Writes data for the source route option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="enable">The enable flag.</param>
        public static void WriteSrcRteOption(this DhcpMessageBuffer buffer, bool enable)
        {
            WriteFlag(buffer, DhcpOptionTag.SrcRte, enable);
        }

        /// <summary>
        /// Writes data for the policy filter option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first destination IP.</param>
        /// <param name="mask1">The first destination mask.</param>
        public static void WritePolicyFilterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 mask1)
        {
            WriteIPs(buffer, DhcpOptionTag.PolicyFilter, ip1, mask1);
        }

        /// <summary>
        /// Writes data for the policy filter option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first destination IP.</param>
        /// <param name="mask1">The first destination mask.</param>
        /// <param name="ip2">The second destination IP.</param>
        /// <param name="mask2">The second destination mask.</param>
        public static void WritePolicyFilterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 mask1, IPAddressV4 ip2, IPAddressV4 mask2)
        {
            WriteIPs(buffer, DhcpOptionTag.PolicyFilter, ip1, mask1, ip2, mask2);
        }

        /// <summary>
        /// Writes data for the max datagram reassembly option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size">The max size.</param>
        public static void WriteMaxDGAssemblyOption(this DhcpMessageBuffer buffer, ushort size)
        {
            WriteUInt16(buffer, DhcpOptionTag.MaxDGAssembly, size);
        }

        /// <summary>
        /// Writes data for the default IP time to live option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ttl">The time to live.</param>
        public static void WriteDefaultIPTtlOption(this DhcpMessageBuffer buffer, byte ttl)
        {
            WriteUInt8(buffer, DhcpOptionTag.DefaultIPTtl, ttl);
        }

        /// <summary>
        /// Writes data for the MTU timeout option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="timeout">The timeout in seconds.</param>
        public static void WriteMtuTimeoutOption(this DhcpMessageBuffer buffer, uint timeout)
        {
            WriteUInt32(buffer, DhcpOptionTag.MtuTimeout, timeout);
        }

        /// <summary>
        /// Writes data for the MTU plateau option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size1">The first size (smallest).</param>
        public static void WriteMtuPlateauOption(this DhcpMessageBuffer buffer, ushort size1)
        {
            WriteUInt16(buffer, DhcpOptionTag.MtuPlateau, size1);
        }

        /// <summary>
        /// Writes data for the MTU plateau option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size1">The first size (smallest).</param>
        /// <param name="size2">The second size (largest).</param>
        public static void WriteMtuPlateauOption(this DhcpMessageBuffer buffer, ushort size1, ushort size2)
        {
            WriteUInt16(buffer, DhcpOptionTag.MtuPlateau, size1, size2);
        }

        /// <summary>
        /// Writes data for the MTU plateau option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size1">The first size (smallest).</param>
        /// <param name="size2">The second size.</param>
        /// <param name="size3">The third size (largest).</param>
        public static void WriteMtuPlateauOption(this DhcpMessageBuffer buffer, ushort size1, ushort size2, ushort size3)
        {
            WriteUInt16(buffer, DhcpOptionTag.MtuPlateau, size1, size2, size3);
        }

        /// <summary>
        /// Writes data for the MTU plateau option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size1">The first size (smallest).</param>
        /// <param name="size2">The second size.</param>
        /// <param name="size3">The third size.</param>
        /// <param name="size4">The fourth size (largest).</param>
        public static void WriteMtuPlateauOption(this DhcpMessageBuffer buffer, ushort size1, ushort size2, ushort size3, ushort size4)
        {
            WriteUInt16(buffer, DhcpOptionTag.MtuPlateau, size1, size2, size3, size4);
        }

        /// <summary>
        /// Writes data for the MTU interface option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="mtu">The MTU value.</param>
        public static void WriteMtuInterfaceOption(this DhcpMessageBuffer buffer, ushort mtu)
        {
            WriteUInt16(buffer, DhcpOptionTag.MtuInterface, mtu);
        }

        /// <summary>
        /// Writes data for the MTU subnet option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="allSubnetsAreLocal">Specifies whether all subnets are local.</param>
        public static void WriteMtuSubnetOption(this DhcpMessageBuffer buffer, bool allSubnetsAreLocal)
        {
            WriteFlag(buffer, DhcpOptionTag.MtuSubnet, allSubnetsAreLocal);
        }

        /// <summary>
        /// Writes data for the broadcast address option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="address">The broadcast address.</param>
        public static void WriteBroadcastAddressOption(this DhcpMessageBuffer buffer, IPAddressV4 address)
        {
            WriteIPs(buffer, DhcpOptionTag.BroadcastAddress, address);
        }

        /// <summary>
        /// Writes data for the mask discovery option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="performMaskDiscovery">Specifies whether the client should perform mask discovery.</param>
        public static void WriteMaskDiscoveryOption(this DhcpMessageBuffer buffer, bool performMaskDiscovery)
        {
            WriteFlag(buffer, DhcpOptionTag.MaskDiscovery, performMaskDiscovery);
        }

        /// <summary>
        /// Writes data for the mask supplier option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="shouldRespond">Specifies whether the client should respond to subnet mask requests.</param>
        public static void WriteMaskSupplierOption(this DhcpMessageBuffer buffer, bool shouldRespond)
        {
            WriteFlag(buffer, DhcpOptionTag.MaskSupplier, shouldRespond);
        }

        /// <summary>
        /// Writes data for the mask supplier option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="useTrailers">Specifies whether the client should perform router discovery.</param>
        public static void WriteRouterDiscoveryOption(this DhcpMessageBuffer buffer, bool useTrailers)
        {
            WriteFlag(buffer, DhcpOptionTag.RouterDiscovery, useTrailers);
        }

        /// <summary>
        /// Writes data for the router request option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="address">The router solicitation address.</param>
        public static void WriteRouterRequestOption(this DhcpMessageBuffer buffer, IPAddressV4 address)
        {
            WriteIPs(buffer, DhcpOptionTag.RouterRequest, address);
        }

        /// <summary>
        /// Writes data for the static route option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first destination IP.</param>
        /// <param name="router1">The first router.</param>
        public static void WriteStaticRouteOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 router1)
        {
            WriteIPs(buffer, DhcpOptionTag.StaticRoute, ip1, router1);
        }

        /// <summary>
        /// Writes data for the static route option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first destination IP.</param>
        /// <param name="router1">The first router.</param>
        /// <param name="ip2">The second destination IP.</param>
        /// <param name="router2">The second router.</param>
        public static void WriteStaticRouteOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 router1, IPAddressV4 ip2, IPAddressV4 router2)
        {
            WriteIPs(buffer, DhcpOptionTag.StaticRoute, ip1, router1, ip2, router2);
        }

        /// <summary>
        /// Writes data for the trailers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="useTrailers">Specifies whether the client should use trailers.</param>
        public static void WriteTrailersOption(this DhcpMessageBuffer buffer, bool useTrailers)
        {
            WriteFlag(buffer, DhcpOptionTag.Trailers, useTrailers);
        }

        /// <summary>
        /// Writes data for the ARP timeout option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="timeout">The ARP cache timeout in seconds.</param>
        public static void WriteArpTimeoutOption(this DhcpMessageBuffer buffer, uint timeout)
        {
            WriteUInt32(buffer, DhcpOptionTag.ArpTimeout, timeout);
        }

        /// <summary>
        /// Writes data for the Ethernet option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="useRfc1042">Specifies whether the client should use IEEE 802.3 (RFC 1042) encapsulation.</param>
        public static void WriteEthernetOption(this DhcpMessageBuffer buffer, bool useRfc1042)
        {
            WriteFlag(buffer, DhcpOptionTag.Ethernet, useRfc1042);
        }

        /// <summary>
        /// Writes data for the default TCP time to live option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ttl">The time to live.</param>
        public static void WriteDefaultTcpTtlOption(this DhcpMessageBuffer buffer, byte ttl)
        {
            WriteUInt8(buffer, DhcpOptionTag.DefaultTcpTtl, ttl);
        }

        /// <summary>
        /// Writes data for the keepalive time option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="interval">The interval in seconds.</param>
        public static void WriteKeepaliveTimeOption(this DhcpMessageBuffer buffer, uint interval)
        {
            WriteUInt32(buffer, DhcpOptionTag.KeepaliveTime, interval);
        }

        /// <summary>
        /// Writes data for the keepalive data option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="sendGarbage">Specifies whether the client should send keepalive messages with a garbage octet.</param>
        public static void WriteKeepaliveDataOption(this DhcpMessageBuffer buffer, bool sendGarbage)
        {
            WriteFlag(buffer, DhcpOptionTag.KeepaliveData, sendGarbage);
        }

        /// <summary>
        /// Writes ASCII encoded data for the NIS domain option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="domainName">The NIS domain name.</param>
        public static void WriteNisDomainOption(this DhcpMessageBuffer buffer, string domainName)
        {
            buffer.WriteOption(DhcpOptionTag.NisDomain, domainName, Encoding.ASCII);
        }

        /// <summary>
        /// Writes data for the NIS servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NIS server IP.</param>
        public static void WriteNisServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.NisServers, ip1);
        }

        /// <summary>
        /// Writes data for the NIS servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NIS server IP.</param>
        /// <param name="ip2">The second NIS server IP.</param>
        public static void WriteNisServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.NisServers, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the NIS servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NIS server IP.</param>
        /// <param name="ip2">The second NIS server IP.</param>
        /// <param name="ip3">The third NIS server IP.</param>
        public static void WriteNisServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.NisServers, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the NIS servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NIS server IP.</param>
        /// <param name="ip2">The second NIS server IP.</param>
        /// <param name="ip3">The third NIS server IP.</param>
        /// <param name="ip4">The fourth NIS server IP.</param>
        public static void WriteNisServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.NisServers, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the NTP servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NTP server IP.</param>
        public static void WriteNtpServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.NtpServers, ip1);
        }

        /// <summary>
        /// Writes data for the NTP servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NTP server IP.</param>
        /// <param name="ip2">The second NTP server IP.</param>
        public static void WriteNtpServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.NtpServers, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the NTP servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NTP server IP.</param>
        /// <param name="ip2">The second NTP server IP.</param>
        /// <param name="ip3">The third NTP server IP.</param>
        public static void WriteNtpServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.NtpServers, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the NTP servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NTP server IP.</param>
        /// <param name="ip2">The second NTP server IP.</param>
        /// <param name="ip3">The third NTP server IP.</param>
        /// <param name="ip4">The fourth NTP server IP.</param>
        public static void WriteNtpServersOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.NtpServers, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes the header for the vendor specific information option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <returns>A buffer to allow writing vendor specific sub-options.</returns>
        public static DhcpSubOptionsBuffer WriteVendorSpecificOptionHeader(this DhcpMessageBuffer buffer)
        {
            return new DhcpSubOptionsBuffer(buffer, DhcpOptionTag.VendorSpecific);
        }

        /// <summary>
        /// Writes data for the vendor specific information option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="data">The vendor-specific data.</param>
        public static void WriteVendorSpecificOption(this DhcpMessageBuffer buffer, ReadOnlySpan<byte> data)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.VendorSpecific, (byte)data.Length);
            data.CopyTo(option.Data.Span);
        }

        /// <summary>
        /// Writes data for the NetBIOS name servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        public static void WriteNetBiosNameSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosNameSrv, ip1);
        }

        /// <summary>
        /// Writes data for the NetBIOS name servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        public static void WriteNetBiosNameSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosNameSrv, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the NetBIOS name servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        /// <param name="ip3">The third NetBIOS server IP.</param>
        public static void WriteNetBiosNameSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosNameSrv, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the NetBIOS name servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        /// <param name="ip3">The third NetBIOS server IP.</param>
        /// <param name="ip4">The fourth NetBIOS server IP.</param>
        public static void WriteNetBiosNameSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosNameSrv, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the NetBIOS datagram distribution servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        public static void WriteNetBiosDistSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosDistSrv, ip1);
        }

        /// <summary>
        /// Writes data for the NetBIOS datagram distribution servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        public static void WriteNetBiosDistSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosDistSrv, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the NetBIOS datagram distribution servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        /// <param name="ip3">The third NetBIOS server IP.</param>
        public static void WriteNetBiosDistSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosDistSrv, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the NetBIOS datagram distribution servers option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first NetBIOS server IP.</param>
        /// <param name="ip2">The second NetBIOS server IP.</param>
        /// <param name="ip3">The third NetBIOS server IP.</param>
        /// <param name="ip4">The fourth NetBIOS server IP.</param>
        public static void WriteNetBiosDistSrvOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.NetBiosDistSrv, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the NetBIOS node type option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="nodeType">The node type.</param>
        public static void WriteNetBiosNodeTypeOption(this DhcpMessageBuffer buffer, NetBiosNodeType nodeType)
        {
            WriteUInt8(buffer, DhcpOptionTag.NetBiosNodeType, (byte)nodeType);
        }

        /// <summary>
        /// Writes UTF-8 encoded data for the NetBIOS scope option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="scope">The scope buffer.</param>
        public static void WriteNetBiosScopeOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> scope)
        {
            buffer.WriteOption(DhcpOptionTag.NetBiosScope, scope, Encoding.UTF8);
        }

        /// <summary>
        /// Writes data for the X Window font server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window font server IP.</param>
        public static void WriteXWindowFontOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowFont, ip1);
        }

        /// <summary>
        /// Writes data for the X Window font server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window font server IP.</param>
        /// <param name="ip2">The second X Window font server IP.</param>
        public static void WriteXWindowFontOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowFont, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the X Window font server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window font server IP.</param>
        /// <param name="ip2">The second X Window font server IP.</param>
        /// <param name="ip3">The third X Window font server IP.</param>
        public static void WriteXWindowFontOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowFont, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the X Window font server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window font server IP.</param>
        /// <param name="ip2">The second X Window font server IP.</param>
        /// <param name="ip3">The third X Window font server IP.</param>
        /// <param name="ip4">The fourth X Window font server IP.</param>
        public static void WriteXWindowFontOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowFont, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the X Window display manager option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window display manager IP.</param>
        public static void WriteXWindowManagerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowManager, ip1);
        }

        /// <summary>
        /// Writes data for the X Window display manager option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window display manager IP.</param>
        /// <param name="ip2">The second X Window display manager IP.</param>
        public static void WriteXWindowManagerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowManager, ip1, ip2);
        }

        /// <summary>
        /// Writes data for the X Window display manager option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window display manager IP.</param>
        /// <param name="ip2">The second X Window display manager IP.</param>
        /// <param name="ip3">The third X Window display manager IP.</param>
        public static void WriteXWindowManagerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowManager, ip1, ip2, ip3);
        }

        /// <summary>
        /// Writes data for the X Window display manager option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first X Window display manager IP.</param>
        /// <param name="ip2">The second X Window display manager IP.</param>
        /// <param name="ip3">The third X Window display manager IP.</param>
        /// <param name="ip4">The fourth X Window display manager IP.</param>
        public static void WriteXWindowManagerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            WriteIPs(buffer, DhcpOptionTag.XWindowManager, ip1, ip2, ip3, ip4);
        }

        /// <summary>
        /// Writes data for the requested IP address option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip">The requested IP.</param>
        public static void WriteAddressRequestOption(this DhcpMessageBuffer buffer, IPAddressV4 ip)
        {
            WriteIPs(buffer, DhcpOptionTag.AddressRequest, ip);
        }

        /// <summary>
        /// Writes data for the IP address lease time option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="leaseTime">The lease time in seconds.</param>
        public static void WriteAddressTimeOption(this DhcpMessageBuffer buffer, uint leaseTime)
        {
            WriteUInt32(buffer, DhcpOptionTag.AddressTime, leaseTime);
        }

        /// <summary>
        /// Writes data for the DHCP message type option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="messageType">The message type.</param>
        public static void WriteDhcpMsgTypeOption(this DhcpMessageBuffer buffer, DhcpMessageType messageType)
        {
            WriteUInt8(buffer, DhcpOptionTag.DhcpMsgType, (byte)messageType);
        }

        /// <summary>
        /// Writes data for the DHCP server identification option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip">The server IP.</param>
        public static void WriteDhcpServerIdOption(this DhcpMessageBuffer buffer, IPAddressV4 ip)
        {
            WriteIPs(buffer, DhcpOptionTag.DhcpServerId, ip);
        }

        /// <summary>
        /// Writes data for the requested parameter list option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="p1">The first requested parameter.</param>
        public static void WriteParameterListOption(this DhcpMessageBuffer buffer, DhcpOptionTag p1)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ParameterList, 1);
            ((byte)p1).CopyTo(option.Data, 0);
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
            ((byte)p1).CopyTo(option.Data, 0);
            ((byte)p2).CopyTo(option.Data, 1);
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
            ((byte)p1).CopyTo(option.Data, 0);
            ((byte)p2).CopyTo(option.Data, 1);
            ((byte)p3).CopyTo(option.Data, 2);
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
            ((byte)p1).CopyTo(option.Data, 0);
            ((byte)p2).CopyTo(option.Data, 1);
            ((byte)p3).CopyTo(option.Data, 2);
            ((byte)p4).CopyTo(option.Data, 3);
        }

        /// <summary>
        /// Writes ASCII encoded data for the DHCP error message option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="text">The error message text.</param>
        public static void WriteDhcpMessageOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> text)
        {
            buffer.WriteOption(DhcpOptionTag.DhcpMessage, text, Encoding.ASCII);
        }

        /// <summary>
        /// Writes data for the DHCP maximum message size option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="size">The maximum message size.</param>
        public static void WriteDhcpMaxMsgSizeOption(this DhcpMessageBuffer buffer, ushort size)
        {
            WriteUInt16(buffer, DhcpOptionTag.DhcpMaxMsgSize, size);
        }

        /// <summary>
        /// Writes data for the DHCP renewal (T1) time.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="time">The time in seconds.</param>
        public static void WriteRenewalTimeOption(this DhcpMessageBuffer buffer, uint time)
        {
            WriteUInt32(buffer, DhcpOptionTag.RenewalTime, time);
        }

        /// <summary>
        /// Writes data for the DHCP rebinding (T2) time.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="time">The time in seconds.</param>
        public static void WriteRebindingTimeOption(this DhcpMessageBuffer buffer, uint time)
        {
            WriteUInt32(buffer, DhcpOptionTag.RebindingTime, time);
        }

        /// <summary>
        /// Writes ASCII encoded data for the class identifier option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="id">The identifier.</param>
        public static void WriteClassIdOption(this DhcpMessageBuffer buffer, ReadOnlySpan<char> id)
        {
            buffer.WriteOption(DhcpOptionTag.ClassId, id, Encoding.ASCII);
        }

        /// <summary>
        /// Writes data for the client identifier option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="id">The identifier.</param>
        public static void WriteClientIdOption(this DhcpMessageBuffer buffer, MacAddress id)
        {
            var option = buffer.WriteOptionHeader(DhcpOptionTag.ClientId, 7);
            ((byte)DhcpHardwareAddressType.Ethernet10Mb).CopyTo(option.Data, 0);
            id.CopyTo(option.Data.Slice(1).Span);
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

        private static void WriteFlag(DhcpMessageBuffer buffer, DhcpOptionTag tag, bool flag)
        {
            WriteUInt8(buffer, tag, (byte)(flag ? 1 : 0));
        }

        private static void WriteUInt8(DhcpMessageBuffer buffer, DhcpOptionTag tag, byte value)
        {
            var option = buffer.WriteOptionHeader(tag, 1);
            value.CopyTo(option.Data, 0);
        }

        private static void WriteUInt16(DhcpMessageBuffer buffer, DhcpOptionTag tag, ushort v1)
        {
            var option = buffer.WriteOptionHeader(tag, 2);
            v1.CopyTo(option.Data, 0);
        }

        private static void WriteUInt16(DhcpMessageBuffer buffer, DhcpOptionTag tag, ushort v1, ushort v2)
        {
            var option = buffer.WriteOptionHeader(tag, 4);
            v1.CopyTo(option.Data, 0);
            v2.CopyTo(option.Data, 2);
        }

        private static void WriteUInt16(DhcpMessageBuffer buffer, DhcpOptionTag tag, ushort v1, ushort v2, ushort v3)
        {
            var option = buffer.WriteOptionHeader(tag, 6);
            v1.CopyTo(option.Data, 0);
            v2.CopyTo(option.Data, 2);
            v3.CopyTo(option.Data, 4);
        }

        private static void WriteUInt16(DhcpMessageBuffer buffer, DhcpOptionTag tag, ushort v1, ushort v2, ushort v3, ushort v4)
        {
            var option = buffer.WriteOptionHeader(tag, 8);
            v1.CopyTo(option.Data, 0);
            v2.CopyTo(option.Data, 2);
            v3.CopyTo(option.Data, 4);
            v4.CopyTo(option.Data, 6);
        }

        private static void WriteUInt32(DhcpMessageBuffer buffer, DhcpOptionTag tag, uint value)
        {
            var option = buffer.WriteOptionHeader(tag, 4);
            value.CopyTo(option.Data, 0);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1)
        {
            var option = buffer.WriteOptionHeader(tag, 4);
            ip1.CopyTo(option.Data, 0);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            var option = buffer.WriteOptionHeader(tag, 8);
            ip1.CopyTo(option.Data, 0);
            ip2.CopyTo(option.Data, 4);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            var option = buffer.WriteOptionHeader(tag, 12);
            ip1.CopyTo(option.Data, 0);
            ip2.CopyTo(option.Data, 4);
            ip3.CopyTo(option.Data, 8);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            var option = buffer.WriteOptionHeader(tag, 16);
            ip1.CopyTo(option.Data, 0);
            ip2.CopyTo(option.Data, 4);
            ip3.CopyTo(option.Data, 8);
            ip4.CopyTo(option.Data, 12);
        }
    }
}
