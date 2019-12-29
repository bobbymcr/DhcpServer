// <copyright file="MagicCookie.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The magic cookie value specified in a packet's vendor information field.
    /// </summary>
    public enum MagicCookie : uint
    {
        /// <summary>
        /// An unspecified value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The DHCP magic cookie.
        /// </summary>
        Dhcp = 0x63825363,
    }
}
