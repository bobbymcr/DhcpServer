// <copyright file="DhcpError.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Net.Sockets;

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
            : this(code, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpError"/> struct.
        /// </summary>
        /// <param name="exception">The underlying socket exception.</param>
        public DhcpError(SocketException exception)
            : this(DhcpErrorCode.SocketError, exception)
        {
        }

        private DhcpError(DhcpErrorCode code, SocketException exception)
        {
            this.Code = code;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the DHCP error code.
        /// </summary>
        public DhcpErrorCode Code { get; }

        /// <summary>
        /// Gets the exception associated with the error.
        /// </summary>
        public Exception Exception { get; }
    }
}
