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
            DhcpMessageBuffer messageBuffer = new DhcpMessageBuffer(buffer);
            return this.RunAsync(buffer, messageBuffer, callbacks, token);
        }

        private async Task RunAsync(Memory<byte> buffer, DhcpMessageBuffer messageBuffer, IDhcpReceiveCallbacks callbacks, CancellationToken token)
        {
            while (true)
            {
                try
                {
                    int length = await this.socket.ReceiveAsync(buffer, token);
                    if ((length > ushort.MaxValue) || (length > buffer.Length))
                    {
                        await callbacks.OnErrorAsync(new DhcpError(DhcpErrorCode.PacketTooLarge), token);
                    }
                    else if (messageBuffer.Load((ushort)length))
                    {
                        await callbacks.OnReceiveAsync(messageBuffer, token);
                    }
                    else
                    {
                        await callbacks.OnErrorAsync(new DhcpError(DhcpErrorCode.PacketTooSmall), token);
                    }
                }
                catch (DhcpException e)
                {
                    await callbacks.OnErrorAsync(new DhcpError(e), token);
                }
            }
        }
    }
}
