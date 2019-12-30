// <copyright file="DhcpReceiveLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the ability to continuously receive and process DHCP messages from a datagram socket.
    /// </summary>
    public sealed class DhcpReceiveLoop
    {
        private readonly IInputSocket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpReceiveLoop"/> class.
        /// </summary>
        /// <param name="socket">The input socket where messages will be received.</param>
        public DhcpReceiveLoop(IInputSocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// Runs an asynchronous receive loop.
        /// </summary>
        /// <typeparam name="T">The user-defined object type.</typeparam>
        /// <param name="buffer">The buffer to hold received messages.</param>
        /// <param name="obj">A user-defined parameter object.</param>
        /// <param name="processAsync">The user-defined processing callback.</param>
        /// <returns>A <see cref="Task"/> tracking the asynchronous operation.</returns>
        public async Task RunAsync<T>(Memory<byte> buffer, T obj, Func<DhcpMessageBuffer, T, ValueTask> processAsync)
        {
            DhcpMessageBuffer messageBuffer = new DhcpMessageBuffer(buffer);
            while (true)
            {
                int length = await this.socket.ReceiveAsync(buffer);
                messageBuffer.Load(length);
                await processAsync(messageBuffer, obj);
            }
        }
    }
}
