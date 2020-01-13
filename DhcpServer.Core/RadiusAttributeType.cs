// <copyright file="RadiusAttributeType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The RADIUS attribute type, as defined in
    /// <see href="https://www.iana.org/assignments/radius-types/radius-types.xhtml#radius-types-2"/> .
    /// </summary>
    public enum RadiusAttributeType : byte
    {
        /// <summary>
        /// User-Name
        /// </summary>
        UserName = 1,

        /// <summary>
        /// Service-Type
        /// </summary>
        ServiceType = 6,
    }
}
