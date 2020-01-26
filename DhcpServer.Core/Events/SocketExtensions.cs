// <copyright file="SocketExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains extension methods for <see cref="ISocket"/>.
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Returns a new socket instance augmented with events.
        /// </summary>
        /// <param name="inner">The inner socket.</param>
        /// <param name="id">The socket identifier.</param>
        /// <param name="socketEvents">The events for the socket.</param>
        /// <returns>A new socket instance wrapping the inner socket.</returns>
        public static ISocket WithEvents(this ISocket inner, SocketId id, ISocketEvents socketEvents)
        {
            return new SocketWithEvents(id, inner, socketEvents);
        }

        private sealed class SocketWithEvents : ISocket
        {
            private readonly SocketId id;
            private readonly ISocket inner;
            private readonly ISocketEvents socketEvents;

            public SocketWithEvents(SocketId id, ISocket inner, ISocketEvents socketEvents)
            {
                this.id = id;
                this.inner = inner;
                this.socketEvents = socketEvents;
            }

            public async Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint)
            {
                Guid activityId = ActivityScope.CurrentId;
                this.socketEvents.SendStart(this.id, buffer.Length, endpoint);
                await this.inner.SendAsync(buffer, endpoint);
                using (new ActivityScope(activityId))
                {
                    this.socketEvents.SendEnd(this.id, true, null);
                }
            }

            public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
