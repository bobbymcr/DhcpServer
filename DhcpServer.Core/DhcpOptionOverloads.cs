// <copyright file="DhcpOptionOverloads.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Indicates that the DHCP 'sname' or 'file' fields are being overloaded to carry DHCP options.
     /// </summary>
    [Flags]
    public enum DhcpOptionOverloads : byte
    {
        /// <summary>
        /// No option overload.
        /// </summary>
        None = 0,

        /// <summary>
        /// The 'file' field is used to hold options.
        /// </summary>
        File = 0x1,

        /// <summary>
        /// The 'sname' field is used to hold options.
        /// </summary>
        SName = 0x2,
    }
}
