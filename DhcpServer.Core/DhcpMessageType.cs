// <copyright file="DhcpMessageType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP message type, as defined in
    /// <see href="https://www.iana.org/assignments/bootp-dhcp-parameters/bootp-dhcp-parameters.xhtml#message-type-53"/> .
    /// </summary>
    public enum DhcpMessageType : byte
    {
        /// <summary>
        /// A DHCPDISCOVER message.
        /// </summary>
        Discover = 1,

        /// <summary>
        /// A DHCPOFFER message.
        /// </summary>
        Offer = 2,
    }
}
