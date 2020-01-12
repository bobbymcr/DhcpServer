// <copyright file="DocsisDeviceClass.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// The DOCSIS device class, as defined in RFC 3256.
    /// </summary>
    [Flags]
    public enum DocsisDeviceClass : uint
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0x0,

        /// <summary>
        /// CPE Controlled Cable Modem (CCCM)
        /// </summary>
        CpeControlledCableModem = 0x1,
    }
}
