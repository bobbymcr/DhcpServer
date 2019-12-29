// <copyright file="DhcpOpcode.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP packet opcode.
    /// </summary>
    public enum DhcpOpcode : byte
    {
        /// <summary>
        /// An unspecified opcode.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies a BOOTREQUEST packet.
        /// </summary>
        BootRequest = 1,

        /// <summary>
        /// Specifies a BOOTREPLY packet.
        /// </summary>
        BootReply = 2,
    }
}
