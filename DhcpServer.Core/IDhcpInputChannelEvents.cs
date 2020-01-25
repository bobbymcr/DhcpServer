// <copyright file="IDhcpInputChannelEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannel"/>.
    /// </summary>
    public interface IDhcpInputChannelEvents
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannel.ReceiveAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        void ReceiveStart(int id);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannel.ReceiveAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="succeeded">Whether the operation succeeded or not.</param>
        /// <param name="error">The error returned from the operation.</param>
        /// <param name="exception">The exception that occurred during the operation, or <c>null</c>.</param>
        void ReceiveEnd(int id, bool succeeded, DhcpError error, Exception exception);
    }
}
