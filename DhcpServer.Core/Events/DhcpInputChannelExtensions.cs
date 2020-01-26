// <copyright file="DhcpInputChannelExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains extension methods for <see cref="IDhcpInputChannel"/>.
    /// </summary>
    public static class DhcpInputChannelExtensions
    {
        /// <summary>
        /// Returns a new channel instance augmented with events.
        /// </summary>
        /// <param name="inner">The inner channel.</param>
        /// <param name="id">The channel identifier.</param>
        /// <param name="channelEvents">The events for the channel.</param>
        /// <returns>A new channel instance wrapping the inner channel.</returns>
        public static IDhcpInputChannel WithEvents(
            this IDhcpInputChannel inner,
            DhcpChannelId id,
            IDhcpInputChannelEvents channelEvents = null)
        {
            if (channelEvents != null)
            {
                return new DhcpInputChannelWithEvents(inner, id, channelEvents);
            }

            return inner;
        }

        private sealed class DhcpInputChannelWithEvents : IDhcpInputChannel
        {
            private readonly IDhcpInputChannel inner;
            private readonly DhcpChannelId id;
            private readonly IDhcpInputChannelEvents channelEvents;

            public DhcpInputChannelWithEvents(
                IDhcpInputChannel inner,
                DhcpChannelId id,
                IDhcpInputChannelEvents channelEvents)
            {
                this.inner = inner;
                this.id = id;
                this.channelEvents = channelEvents;
            }

            public async Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token)
            {
                this.channelEvents.ReceiveStart(this.id);
                Guid activityId = ActivityScope.CurrentId;
                try
                {
                    var result = await this.inner.ReceiveAsync(token);
                    this.OnEnd(activityId, result.Item2, null);
                    return result;
                }
                catch (Exception e)
                {
                    this.OnEnd(activityId, default, e);
                    throw;
                }
            }

            private void OnEnd(Guid activityId, DhcpError error, Exception exception)
            {
                using (new ActivityScope(activityId))
                {
                    bool succeeded = (error.Code == DhcpErrorCode.None) && (exception == null);
                    this.channelEvents.ReceiveEnd(this.id, succeeded, error, exception);
                }
            }
        }
    }
}