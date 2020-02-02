// <copyright file="ISocketEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
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
        /// <param name="status">The operation status.</param>
        void SendEnd(SocketId id, OperationStatus status);

        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IInputSocket.ReceiveAsync(System.Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        void ReceiveStart(SocketId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IInputSocket.ReceiveAsync(System.Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="result">The result.</param>
        /// <param name="status">The operation status.</param>
        void ReceiveEnd(SocketId id, int result, OperationStatus status);

        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="System.IDisposable.Dispose"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        void DisposeStart(SocketId id);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="System.IDisposable.Dispose"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        void DisposeEnd(SocketId id);
    }
}
