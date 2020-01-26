// <copyright file="ISocketEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;

    /// <summary>
    /// Operational events for an <see cref="ISocket"/>.
    /// </summary>
    public interface ISocketEvents
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IOutputSocket.SendAsync(System.ReadOnlyMemory{byte}, IPEndpointV4)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <param name="endpoint">The endpoint.</param>
        void SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IOutputSocket.SendAsync(System.ReadOnlyMemory{byte}, IPEndpointV4)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="succeeded">Whether the operation succeeded or not.</param>
        /// <param name="exception">The exception that occurred during the operation, or <c>null</c>.</param>
        void SendEnd(SocketId id, bool succeeded, Exception exception);

        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IInputSocket.ReceiveAsync(Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        void ReceiveStart(SocketId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IInputSocket.ReceiveAsync(Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="result">The result.</param>
        /// <param name="succeeded">Whether the operation succeeded or not.</param>
        /// <param name="exception">The exception that occurred during the operation, or <c>null</c>.</param>
        void ReceiveEnd(SocketId id, int result, bool succeeded, Exception exception);
    }
}
