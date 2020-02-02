// <copyright file="IDhcpInputChannelFactoryEvents{TState}.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannelFactory"/> with user-defined state.
    /// </summary>
    /// <typeparam name="TState">The user-defined state.</typeparam>
    public interface IDhcpInputChannelFactoryEvents<TState>
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <returns>The user-defined state object.</returns>
        TState CreateChannelStart(DhcpChannelId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="status">The operation status.</param>
        /// <param name="state">The user-defined state object from <see cref="CreateChannelStart(DhcpChannelId, int)"/>.</param>
        void CreateChannelEnd(DhcpChannelId id, OperationStatus status, TState state);
    }
}
