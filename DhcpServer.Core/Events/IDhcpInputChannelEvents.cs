// <copyright file="IDhcpInputChannelEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
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
        void ReceiveStart(DhcpChannelId id);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannel.ReceiveAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="error">The error returned from the operation.</param>
        /// <param name="status">The operation status.</param>
        void ReceiveEnd(DhcpChannelId id, DhcpError error, OperationStatus status);
    }
}
