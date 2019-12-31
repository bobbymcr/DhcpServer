// <copyright file="IOutputSocket.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a datagram output socket.
    /// </summary>
    public interface IOutputSocket
    {
        /// <summary>
        /// Sends data to a specific network endpoint.
        /// </summary>
        /// <param name="buffer">A region of memory that contains the data to send.</param>
        /// <param name="endpoint">The destination endpoint.</param>
        /// <returns>A <see cref="Task"/> tracking the asynchronous operation.</returns>
        Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint);
    }
}
