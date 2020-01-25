﻿// <copyright file="DhcpInputChannelFactory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Creates <see cref="DhcpInputChannel"/> instances using a shared input socket.
    /// </summary>
    public sealed class DhcpInputChannelFactory
    {
        private readonly IInputSocket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpInputChannelFactory"/> class.
        /// </summary>
        /// <param name="socket">The input socket to be shared by all instances.</param>
        public DhcpInputChannelFactory(IInputSocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// Creates a new input channel instance.
        /// </summary>
        /// <param name="rawBuffer">The underlying buffer for received data.</param>
        /// <returns>The channel instance.</returns>
        public IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer)
        {
            return new DhcpInputChannel(this.socket, rawBuffer);
        }
    }
}
