﻿// <copyright file="DhcpInputChannelFactoryExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
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
        /// <param name="channelEvents">The events for the channel.</param>
        /// <returns>A new factory instance wrapping the inner factory.</returns>
        public static IDhcpInputChannelFactory WithEvents(
            this IDhcpInputChannelFactory inner,
            IDhcpInputChannelFactoryEvents factoryEvents = null,
            IDhcpInputChannelEvents channelEvents = null)
        {
            IDhcpInputChannelFactoryEvents<bool> factoryEventsT = (factoryEvents != null) ?
                new DhcpInputChannelFactoryEventsAdapter(factoryEvents) :
                null;
            IDhcpInputChannelEvents<bool> channelEventsT = (channelEvents != null) ?
                new DhcpInputChannelEventsAdapter(channelEvents) :
                null;
            if ((factoryEventsT != null) || (channelEventsT != null))
            {
                return new DhcpInputChannelFactoryWithEvents<bool>(inner, factoryEventsT, channelEventsT);
            }

            return inner;
        }

        /// <summary>
        /// Returns a new channel factory instance augmented with events with user-defined state.
        /// </summary>
        /// <typeparam name="TState">The user-defined state.</typeparam>
        /// <param name="inner">The inner factory.</param>
        /// <param name="factoryEvents">The events for the factory.</param>
        /// <param name="channelEvents">The events for the channel.</param>
        /// <returns>A new factory instance wrapping the inner factory.</returns>
        public static IDhcpInputChannelFactory WithEvents<TState>(
            this IDhcpInputChannelFactory inner,
            IDhcpInputChannelFactoryEvents<TState> factoryEvents = null,
            IDhcpInputChannelEvents<TState> channelEvents = null)
        {
            if ((factoryEvents != null) || (channelEvents != null))
            {
                return new DhcpInputChannelFactoryWithEvents<TState>(inner, factoryEvents, channelEvents);
            }

            return inner;
        }

        private sealed class DhcpInputChannelFactoryEventsAdapter : IDhcpInputChannelFactoryEvents<bool>
        {
            private readonly IDhcpInputChannelFactoryEvents inner;

            public DhcpInputChannelFactoryEventsAdapter(IDhcpInputChannelFactoryEvents inner)
            {
                this.inner = inner;
            }

            public bool CreateChannelStart(DhcpChannelId id, int bufferSize)
            {
                this.inner.CreateChannelStart(id, bufferSize);
                return false;
            }

            public void CreateChannelEnd(DhcpChannelId id, OperationStatus status, bool state)
            {
                this.inner.CreateChannelEnd(id, status);
            }
        }

        private sealed class DhcpInputChannelFactoryWithEvents<TState> : IDhcpInputChannelFactory
        {
            private readonly IDhcpInputChannelFactory inner;
            private readonly IDhcpInputChannelFactoryEvents<TState> factoryEvents;
            private readonly IDhcpInputChannelEvents<TState> channelEvents;

            private int lastId;

            public DhcpInputChannelFactoryWithEvents(
                IDhcpInputChannelFactory inner,
                IDhcpInputChannelFactoryEvents<TState> factoryEvents,
                IDhcpInputChannelEvents<TState> channelEvents)
            {
                this.inner = inner;
                this.factoryEvents = factoryEvents;
                this.channelEvents = channelEvents;
            }

            public IDhcpInputChannel CreateChannel(Memory<byte> rawBuffer)
            {
                int id = Interlocked.Increment(ref this.lastId);
                TState state = default;
                if (this.factoryEvents != null)
                {
                    state = this.factoryEvents.CreateChannelStart(id, rawBuffer.Length);
                }

                try
                {
                    IDhcpInputChannel channel = this.inner.CreateChannel(rawBuffer)
                        .WithEvents(id, this.channelEvents);
                    this.factoryEvents?.CreateChannelEnd(id, OperationStatus.Success(), state);
                    return channel;
                }
                catch (Exception e)
                {
                    this.factoryEvents?.CreateChannelEnd(id, OperationStatus.Failure(e), state);
                    throw;
                }
            }
        }
    }
}