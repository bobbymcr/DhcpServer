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

        /// <summary>
        /// Returns a new socket instance augmented with events with user-defined state.
        /// </summary>
        /// <typeparam name="TState">The user-defined state.</typeparam>
        /// <param name="inner">The inner socket.</param>
        /// <param name="id">The socket identifier.</param>
        /// <param name="socketEvents">The events for the socket.</param>
        /// <returns>A new socket instance wrapping the inner socket.</returns>
        public static ISocket WithEvents<TState>(this ISocket inner, SocketId id, ISocketEvents<TState> socketEvents)
        {
            return new SocketWithEvents<TState>(id, inner, socketEvents);
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
                try
                {
                    this.socketEvents.SendStart(this.id, buffer.Length, endpoint);
                    await this.inner.SendAsync(buffer, endpoint);
                    this.OnEndSend(activityId, null);
                }
                catch (Exception e)
                {
                    this.OnEndSend(activityId, e);
                    throw;
                }
            }

            public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                Guid activityId = ActivityScope.CurrentId;
                this.socketEvents.ReceiveStart(this.id, buffer.Length);
                try
                {
                    int result = await this.inner.ReceiveAsync(buffer, token);
                    this.OnEndReceive(activityId, result, null);
                    return result;
                }
                catch (Exception e)
                {
                    this.OnEndReceive(activityId, -1, e);
                    throw;
                }
            }

            public void Dispose()
            {
                this.socketEvents.DisposeStart(this.id);
                this.inner.Dispose();
                this.socketEvents.DisposeEnd(this.id);
            }

            private void OnEndSend(Guid activityId, Exception exception)
            {
                using (new ActivityScope(activityId))
                {
                    this.socketEvents.SendEnd(this.id, exception == null, exception);
                }
            }

            private void OnEndReceive(Guid activityId, int result, Exception exception)
            {
                using (new ActivityScope(activityId))
                {
                    this.socketEvents.ReceiveEnd(this.id, result, exception == null, exception);
                }
            }
        }

        private sealed class SocketWithEvents<TState> : ISocket
        {
            private readonly SocketId id;
            private readonly ISocket inner;
            private readonly ISocketEvents<TState> socketEvents;

            public SocketWithEvents(SocketId id, ISocket inner, ISocketEvents<TState> socketEvents)
            {
                this.id = id;
                this.inner = inner;
                this.socketEvents = socketEvents;
            }

            public async Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint)
            {
                Guid activityId = ActivityScope.CurrentId;
                TState state = this.socketEvents.SendStart(this.id, buffer.Length, endpoint);
                try
                {
                    await this.inner.SendAsync(buffer, endpoint);
                    this.OnEndSend(activityId, null, state);
                }
                catch (Exception e)
                {
                    this.OnEndSend(activityId, e, state);
                    throw;
                }
            }

            public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
            {
                Guid activityId = ActivityScope.CurrentId;
                TState state = this.socketEvents.ReceiveStart(this.id, buffer.Length);
                try
                {
                    int result = await this.inner.ReceiveAsync(buffer, token);
                    this.OnEndReceive(activityId, result, null, state);
                    return result;
                }
                catch (Exception e)
                {
                    this.OnEndReceive(activityId, -1, e, state);
                    throw;
                }
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            private void OnEndSend(Guid activityId, Exception exception, TState state)
            {
                using (new ActivityScope(activityId))
                {
                    this.socketEvents.SendEnd(this.id, exception == null, exception, state);
                }
            }

            private void OnEndReceive(Guid activityId, int result, Exception exception, TState state)
            {
                using (new ActivityScope(activityId))
                {
                    this.socketEvents.ReceiveEnd(this.id, result, exception == null, exception, state);
                }
            }
        }
    }
}
