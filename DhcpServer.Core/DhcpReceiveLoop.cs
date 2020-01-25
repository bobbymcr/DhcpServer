// <copyright file="DhcpReceiveLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the ability to continuously receive and process DHCP messages from one or more channels.
    /// </summary>
    public sealed class DhcpReceiveLoop
    {
        private readonly Func<Memory<byte>, IDhcpInputChannel> channelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpReceiveLoop"/> class.
        /// </summary>
        /// <param name="channelFactory">The input channel factory.</param>
        public DhcpReceiveLoop(Func<Memory<byte>, IDhcpInputChannel> channelFactory)
        {
            this.channelFactory = channelFactory;
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
            IDhcpInputChannel channel = this.channelFactory(buffer);
            return this.RunAsync(channel, callbacks, token);
        }

        private async Task RunAsync(IDhcpInputChannel channel, IDhcpReceiveCallbacks callbacks, CancellationToken token)
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
