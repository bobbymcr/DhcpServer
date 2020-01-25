// <copyright file="DhcpInputChannel.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A DHCP input channel based on a datagram socket.
    /// </summary>
    public sealed class DhcpInputChannel : IDhcpInputChannel
    {
        private readonly IInputSocket socket;
        private readonly Memory<byte> rawBuffer;
        private readonly DhcpMessageBuffer buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpInputChannel"/> class.
        /// </summary>
        /// <param name="socket">The input socket.</param>
        /// <param name="rawBuffer">The underlying buffer to hold received data.</param>
        public DhcpInputChannel(IInputSocket socket, Memory<byte> rawBuffer)
        {
            this.socket = socket;
            this.rawBuffer = rawBuffer;
            this.buffer = new DhcpMessageBuffer(this.rawBuffer);
        }

        /// <summary>
        /// Receives a DHCP message from the channel.
        /// </summary>
        /// <param name="token">Used to signal that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the message buffer and, if failed, the error.</returns>
        public async Task<(DhcpMessageBuffer, DhcpError)> ReceiveAsync(CancellationToken token)
        {
            try
            {
                int length = await this.socket.ReceiveAsync(this.rawBuffer, token);
                if ((length > ushort.MaxValue) || (length > this.rawBuffer.Length))
                {
                    return this.Error(DhcpErrorCode.PacketTooLarge);
                }

                if (!this.buffer.Load((ushort)length))
                {
                    return this.Error(DhcpErrorCode.PacketTooSmall);
                }

                return this.OK();
            }
            catch (DhcpException e)
            {
                return this.Error(e);
            }
        }

        private (DhcpMessageBuffer, DhcpError) OK() => (this.buffer, default);

        private (DhcpMessageBuffer, DhcpError) Error(DhcpErrorCode error) => (this.buffer, new DhcpError(error));

        private (DhcpMessageBuffer, DhcpError) Error(DhcpException error) => (this.buffer, new DhcpError(error));
    }
}
