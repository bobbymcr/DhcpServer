// <copyright file="RadiusServiceType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The RADIUS service type, as defined in
    /// <see href="https://www.iana.org/assignments/radius-types/radius-types.xhtml#radius-types-4"/> .
    /// </summary>
    public enum RadiusServiceType : uint
    {
        /// <summary>
        /// Login
        /// </summary>
        Login = 1,
    }
}
