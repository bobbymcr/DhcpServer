// <copyright file="DhcpException.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Represents DHCP errors that occur during execution.
    /// </summary>
    public class DhcpException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpException"/> class.
        /// </summary>
        /// <param name="code">The DHCP error code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DhcpException(DhcpErrorCode code, Exception innerException)
            : base("A DHCP error occurred.", innerException)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets the DHCP error code associated with this exception.
        /// </summary>
        public DhcpErrorCode Code { get; }
    }
}
