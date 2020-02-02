// <copyright file="IDhcpInputChannelFactoryEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannelFactory"/>.
    /// </summary>
    public interface IDhcpInputChannelFactoryEvents
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="bufferSize">The buffer size.</param>
        void CreateChannelStart(DhcpChannelId id, int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="id">The channel identifier.</param>
        /// <param name="status">The operation status.</param>
        void CreateChannelEnd(DhcpChannelId id, OperationStatus status);
    }
}
