// <copyright file="DhcpOptionTag.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP option tag, as defined in
    /// <see href="https://www.iana.org/assignments/bootp-dhcp-parameters/bootp-dhcp-parameters.xhtml#options"/> .
    /// </summary>
    public enum DhcpOptionTag : byte
    {
        /// <summary>
        /// Zero-value options buffer padding.
        /// </summary>
        Pad = 0,

        /// <summary>
        /// Subnet Mask Value
        /// </summary>
        SubnetMask = 1,

        /// <summary>
        /// Router addresses
        /// </summary>
        Router = 3,

        /// <summary>
        /// DNS Server addresses
        /// </summary>
        DomainServer = 6,

        /// <summary>
        /// Requested IP Address
        /// </summary>
        AddressRequest = 50,

        /// <summary>
        /// IP Address Lease Time
        /// </summary>
        AddressTime = 51,

        /// <summary>
        /// Overload "sname" or "file"
        /// </summary>
        Overload = 52,

        /// <summary>
        /// DHCP Message Type
        /// </summary>
        DhcpMsgType = 53,

        /// <summary>
        /// DHCP Server Identification
        /// </summary>
        DhcpServerId = 54,

        /// <summary>
        /// Parameter Request List
        /// </summary>
        ParameterList = 55,

        /// <summary>
        /// DHCP Error Message
        /// </summary>
        DhcpMessage = 56,

        /// <summary>
        /// DHCP Maximum Message Size
        /// </summary>
        DhcpMaxMsgSize = 57,

        /// <summary>
        /// Client Identifier
        /// </summary>
        ClientId = 61,

        /// <summary>
        /// Marks the end of an options buffer.
        /// </summary>
        End = 255,
    }
}
