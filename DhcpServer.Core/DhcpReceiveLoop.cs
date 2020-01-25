// <copyright file="DhcpReceiveLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading;
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
        /// <param name="buffer">The buffer to hold received messages.</param>
        /// <param name="callbacks">A user-defined callback implementation.</param>
        /// <param name="token">Used to signal that the loop should be canceled.</param>
        /// <returns>A <see cref="Task"/> tracking the asynchronous operation.</returns>
        public Task RunAsync(Memory<byte> buffer, IDhcpReceiveCallbacks callbacks, CancellationToken token)
        {
            DhcpInputChannel channel = new DhcpInputChannel(this.socket, buffer);
            return this.RunAsync(channel, callbacks, token);
        }

        private async Task RunAsync(DhcpInputChannel channel, IDhcpReceiveCallbacks callbacks, CancellationToken token)
        {
            while (true)
            {
                (DhcpMessageBuffer buffer, DhcpError error) = await channel.ReceiveAsync(token);
                if (error.Code != DhcpErrorCode.None)
                {
                    await callbacks.OnErrorAsync(error, token);
                }
                else
                {
                    await callbacks.OnReceiveAsync(buffer, token);
                }
            }
        }
    }
}
