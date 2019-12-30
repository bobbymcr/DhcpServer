// <copyright file="IInputSocket.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a datagram input socket.
    /// </summary>
    public interface IInputSocket
    {
        /// <summary>
        /// Receives data from the socket.
        /// </summary>
        /// <param name="buffer">A region of memory that is the storage location for the received data.</param>
        /// <returns>A <see cref="ValueTask{Int32}"/> that completes with the number of bytes received.</returns>
        ValueTask<int> ReceiveAsync(Memory<byte> buffer);
    }
}
