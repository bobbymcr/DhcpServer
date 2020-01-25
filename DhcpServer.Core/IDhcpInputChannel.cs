// <copyright file="IDhcpInputChannel.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a strongly-typed DHCP input channel.
    /// </summary>
    public interface IDhcpInputChannel
    {
        /// <summary>
        /// Receives a DHCP message from the channel.
        /// </summary>
        /// <param name="token">Used to signal that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the message buffer and, if failed, the error.</returns>
        Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token);
    }
}
