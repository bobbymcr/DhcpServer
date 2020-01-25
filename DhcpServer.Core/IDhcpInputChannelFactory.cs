// <copyright file="IDhcpInputChannelFactory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A factory that creates DHCP input channels.
    /// </summary>
    public interface IDhcpInputChannelFactory
    {
        /// <summary>
        /// Creates a DHCP input channel instance.
        /// </summary>
        /// <param name="rawBuffer">The underlying buffer for received data.</param>
        /// <returns>The channel instance.</returns>
        IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer);
    }
}
