// <copyright file="IDhcpInputChannelFactoryEvents{TState}.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;

    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannelFactory"/> with user-defined state.
    /// </summary>
    /// <typeparam name="TState">The user-defined state.</typeparam>
    public interface IDhcpInputChannelFactoryEvents<TState>
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        /// <returns>The user-defined state object.</returns>
        TState CreateChannelStart(DhcpChannelId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="succeeded">Whether the operation succeeded or not.</param>
        /// <param name="exception">The exception that occurred during the operation, or <c>null</c>.</param>
        /// <param name="state">The user-defined state object from <see cref="CreateChannelStart(DhcpChannelId, int)"/>.</param>
        void CreateChannelEnd(DhcpChannelId id, bool succeeded, Exception exception, TState state);
    }
}
