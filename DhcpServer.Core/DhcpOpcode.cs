// <copyright file="DhcpOpcode.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP packet operation code, as defined in
    /// <see href="https://www.iana.org/assignments/arp-parameters/arp-parameters.xhtml#arp-parameters-1"/> .
    /// </summary>
    public enum DhcpOpcode : byte
    {
        /// <summary>
        /// An unspecified opcode.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies a REQUEST packet.
        /// </summary>
        Request = 1,

        /// <summary>
        /// Specifies a REPLY packet.
        /// </summary>
        Reply = 2,
    }
}
