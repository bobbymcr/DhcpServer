// <copyright file="DhcpMessageBufferExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
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

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1)
        {
            var option = buffer.WriteOption(tag, 4);
            ip1.WriteTo(option.Data);
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            var option = buffer.WriteOption(tag, 8);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3)
        {
            var option = buffer.WriteOption(tag, 12);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
        }

        private static void WriteIPs(DhcpMessageBuffer buffer, DhcpOptionTag tag, IPAddressV4 ip1, IPAddressV4 ip2, IPAddressV4 ip3, IPAddressV4 ip4)
        {
            var option = buffer.WriteOption(tag, 16);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
            ip4.WriteTo(option.Data.Slice(12));
        }
    }
}
