// <copyright file="ISocketEvents{TState}.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// Operational events for an <see cref="ISocket"/> with user-defined state.
    /// </summary>
    /// <typeparam name="TState">The user-defined state.</typeparam>
    public interface ISocketEvents<TState>
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IOutputSocket.SendAsync(System.ReadOnlyMemory{byte}, IPEndpointV4)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>The user-defined state object.</returns>
        TState SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IOutputSocket.SendAsync(System.ReadOnlyMemory{byte}, IPEndpointV4)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="status">The operation status.</param>
        /// <param name="state">The user-defined state object from <see cref="SendStart(SocketId, int, IPEndpointV4)"/>.</param>
        void SendEnd(SocketId id, OperationStatus status, TState state);

        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IInputSocket.ReceiveAsync(System.Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <returns>The user-defined state object.</returns>
        TState ReceiveStart(SocketId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IInputSocket.ReceiveAsync(System.Memory{byte}, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="result">The result.</param>
        /// <param name="status">The operation status.</param>
        /// <param name="state">The user-defined state object from <see cref="ReceiveStart(SocketId, int)"/>.</param>
        void ReceiveEnd(SocketId id, int result, OperationStatus status, TState state);

        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="System.IDisposable.Dispose"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <returns>The user-defined state object.</returns>
        TState DisposeStart(SocketId id);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="System.IDisposable.Dispose"/>.
        /// </summary>
        /// <param name="id">The socket identifier.</param>
        /// <param name="state">The user-defined state object from <see cref="DisposeStart(SocketId)"/>.</param>
        void DisposeEnd(SocketId id, TState state);
    }
}
