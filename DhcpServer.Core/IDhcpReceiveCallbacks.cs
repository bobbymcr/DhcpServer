// <copyright file="IDhcpReceiveCallbacks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines callbacks for a DHCP receiver.
    /// </summary>
    public interface IDhcpReceiveCallbacks
    {
        /// <summary>
        /// Called when a new DHCP message is received.
        /// </summary>
        /// <param name="message">The incoming message buffer.</param>
        /// <param name="token">Used to signal that processing should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> to track the asynchronous operation.</returns>
        ValueTask OnReceiveAsync(DhcpMessageBuffer message, CancellationToken token);
    }
}
