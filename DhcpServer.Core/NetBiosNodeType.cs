// <copyright file="NetBiosNodeType.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The NetBIOS node type, as defined in RFC 1001.
    /// </summary>
    public enum NetBiosNodeType : byte
    {
        /// <summary>
        /// An unspecified node type.
        /// </summary>
        None = 0,

        /// <summary>
        /// A broadcast (B) node.
        /// </summary>
        Broadcast = 0x1,

        /// <summary>
        /// A point-to-point (P) node.
        /// </summary>
        PointToPoint = 0x2,

        /// <summary>
        /// A mixed mode (M) node.
        /// </summary>
        MixedMode = 0x4,

        /// <summary>
        /// A hybrid (H) node.
        /// </summary>
        Hybrid = 0x8,
    }
}
