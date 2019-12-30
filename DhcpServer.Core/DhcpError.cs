// <copyright file="DhcpError.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// A value type which holds DHCP error information.
    /// </summary>
    public readonly struct DhcpError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpError"/> struct.
        /// </summary>
        /// <param name="code">The DHCP error code.</param>
        public DhcpError(DhcpErrorCode code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets the DHCP error code.
        /// </summary>
        public DhcpErrorCode Code { get; }
    }
}
