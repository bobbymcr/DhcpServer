// <copyright file="DhcpSocket.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A bound DHCP datagram socket.
    /// </summary>
    public sealed class DhcpSocket : ISocket
    {
        private readonly EndpointCache endpoints;
        private readonly Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpSocket"/> class.
        /// </summary>
        /// <param name="endpoint">The listening endpoint.</param>
        public DhcpSocket(IPEndpointV4 endpoint)
        {
            try
            {
                this.endpoints = new EndpointCache();
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                this.socket.Bind(new IPEndPoint((uint)endpoint.Address, endpoint.Port));
            }
            catch (Exception)
            {
                this.Dispose();
                throw;
            }
        }

        /// <inheritdoc/>
        public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
        {
            try
            {
                return await this.socket.ReceiveAsync(buffer, SocketFlags.None, token);
            }
            catch (SocketException e)
            {
                throw new DhcpException(DhcpErrorCode.SocketError, e);
            }
        }

        /// <inheritdoc/>
        public Task SendAsync(ReadOnlyMemory<byte> buffer, IPEndpointV4 endpoint)
        {
            MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> segment);
            return this.socket.SendToAsync(segment, SocketFlags.None, this.endpoints[endpoint]);
        }

        /// <inheritdoc/>
        public void Dispose() => this.socket.Dispose();
    }
}
