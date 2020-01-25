// <copyright file="DhcpInputChannelFactoryExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading;

    /// <summary>
    /// Contains extension methods for <see cref="IDhcpInputChannelFactory"/>.
    /// </summary>
    public static class DhcpInputChannelFactoryExtensions
    {
        /// <summary>
        /// Returns a new channel factory instance augmented with events.
        /// </summary>
        /// <param name="inner">The inner factory.</param>
        /// <param name="factoryEvents">The events for the factory.</param>
        /// <returns>A new factory instance wrapping the inner factory.</returns>
        public static IDhcpInputChannelFactory WithEvents(this IDhcpInputChannelFactory inner, IDhcpInputChannelFactoryEvents factoryEvents)
        {
            return new DhcpInputChannelFactoryWithEvents(inner, factoryEvents);
        }

        private sealed class DhcpInputChannelFactoryWithEvents : IDhcpInputChannelFactory
        {
            private readonly IDhcpInputChannelFactory inner;
            private readonly IDhcpInputChannelFactoryEvents factoryEvents;

            private int lastId;

            public DhcpInputChannelFactoryWithEvents(IDhcpInputChannelFactory inner, IDhcpInputChannelFactoryEvents factoryEvents)
            {
                this.inner = inner;
                this.factoryEvents = factoryEvents;
            }

            public IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer)
            {
                int id = Interlocked.Increment(ref this.lastId);
                this.factoryEvents.CreateChannelStart(id, rawBuffer.Length);
                try
                {
                    IDhcpInputChannel channel = this.inner.CreateChannel(rawBuffer);
                    this.factoryEvents.CreateChannelEnd(id, true, null);
                    return channel;
                }
                catch (Exception e)
                {
                    this.factoryEvents.CreateChannelEnd(id, false, e);
                    throw;
                }
            }
        }
    }
}