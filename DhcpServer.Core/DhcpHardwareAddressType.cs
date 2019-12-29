// <copyright file="DhcpHardwareAddressType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP hardware address type, as defined in
    /// <see href="https://www.iana.org/assignments/arp-parameters/arp-parameters.xhtml#arp-parameters-2"/> .
    /// </summary>
    public enum DhcpHardwareAddressType : byte
    {
        /// <summary>
        /// An unspecified hardware address type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Ethernet (10Mb).
        /// </summary>
        Ethernet10Mb = 1,
    }
}
