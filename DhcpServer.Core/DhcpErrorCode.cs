// <copyright file="DhcpErrorCode.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// Classifies a DHCP error.
    /// </summary>
    public enum DhcpErrorCode
    {
        /// <summary>
        /// No error.
        /// </summary>
        None = 0,

        /// <summary>
        /// The packet was too small to be a valid DHCP message.
        /// </summary>
        PacketTooSmall = 1,

        /// <summary>
        /// A socket error occurred.
        /// </summary>
        SocketError = 2,
    }
}
