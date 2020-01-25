// <copyright file="DhcpInputChannel.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class DhcpInputChannel
    {
        private readonly IInputSocket socket;
        private readonly Memory<byte> rawBuffer;
        private readonly DhcpMessageBuffer buffer;

        public DhcpInputChannel(IInputSocket socket, Memory<byte> rawBuffer)
        {
            this.socket = socket;
            this.rawBuffer = rawBuffer;
            this.buffer = new DhcpMessageBuffer(this.rawBuffer);
        }

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
