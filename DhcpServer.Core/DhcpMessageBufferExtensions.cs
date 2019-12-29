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
            var option = buffer.WriteOption(DhcpOptionTag.SubnetMask, 4);
            mask.WriteTo(option.Data);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            var option = buffer.WriteOption(DhcpOptionTag.Router, 4);
            ip1.WriteTo(option.Data);
        }

        /// <summary>
        /// Writes data for the router option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first router IP.</param>
        /// <param name="ip2">The second router IP.</param>
        public static void WriteRouterOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            var option = buffer.WriteOption(DhcpOptionTag.Router, 8);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
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
            var option = buffer.WriteOption(DhcpOptionTag.Router, 12);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
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
            var option = buffer.WriteOption(DhcpOptionTag.Router, 16);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
            ip4.WriteTo(option.Data.Slice(12));
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1)
        {
            var option = buffer.WriteOption(DhcpOptionTag.TimeServer, 4);
            ip1.WriteTo(option.Data);
        }

        /// <summary>
        /// Writes data for the time server option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="ip1">The first time server IP.</param>
        /// <param name="ip2">The second time server IP.</param>
        public static void WriteTimeServerOption(this DhcpMessageBuffer buffer, IPAddressV4 ip1, IPAddressV4 ip2)
        {
            var option = buffer.WriteOption(DhcpOptionTag.TimeServer, 8);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
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
            var option = buffer.WriteOption(DhcpOptionTag.TimeServer, 12);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
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
            var option = buffer.WriteOption(DhcpOptionTag.TimeServer, 16);
            ip1.WriteTo(option.Data);
            ip2.WriteTo(option.Data.Slice(4));
            ip3.WriteTo(option.Data.Slice(8));
            ip4.WriteTo(option.Data.Slice(12));
        }
    }
}
