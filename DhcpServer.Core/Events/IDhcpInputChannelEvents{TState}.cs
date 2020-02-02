// <copyright file="IDhcpInputChannelEvents{TState}.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannel"/> with user-defined state..
    /// </summary>
    /// <typeparam name="TState">The user-defined state.</typeparam>
    public interface IDhcpInputChannelEvents<TState>
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannel.ReceiveAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <returns>The user-defined state object.</returns>
        TState ReceiveStart(DhcpChannelId id);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannel.ReceiveAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="error">The error returned from the operation.</param>
        /// <param name="status">The operation status.</param>
        /// <param name="state">The user-defined state object from <see cref="ReceiveStart(DhcpChannelId)"/>.</param>
        void ReceiveEnd(DhcpChannelId id, DhcpError error, OperationStatus status, TState state);
    }
}
