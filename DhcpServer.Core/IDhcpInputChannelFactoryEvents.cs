// <copyright file="IDhcpInputChannelFactoryEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Operational events for an <see cref="IDhcpInputChannelFactory"/>.
    /// </summary>
    public interface IDhcpInputChannelFactoryEvents
    {
        /// <summary>
        /// Denotes the start of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="bufferSize">The buffer size.</param>
        void CreateChannelStart(int bufferSize);

        /// <summary>
        /// Denotes the end of a call to
        /// <see cref="IDhcpInputChannelFactory.CreateChannel(System.Memory{byte})"/>.
        /// </summary>
        /// <param name="succeeded">Whether the operation succeeded or not.</param>
        /// <param name="exception">The exception that occurred during the operation, or <c>null</c>.</param>
        void CreateChannelEnd(bool succeeded, Exception exception);
    }
}
