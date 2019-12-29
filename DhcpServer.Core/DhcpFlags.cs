// <copyright file="DhcpFlags.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// The DHCP message flags.
    /// </summary>
    [Flags]
    public enum DhcpFlags : ushort
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// The broadcast flag.
        /// </summary>
        Broadcast = 0x8000,
    }
}
