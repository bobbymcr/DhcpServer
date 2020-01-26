// <copyright file="SocketId.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// An identifier for a <see cref="ISocket"/>, used in operational events.
    /// </summary>
    public readonly struct SocketId
    {
        private readonly int id;

        private SocketId(int number)
        {
            this.id = number;
        }

        /// <summary>
        /// Converts a number to a <see cref="SocketId"/>.
        /// </summary>
        /// <param name="number">The numeric identifier.</param>
        public static implicit operator SocketId(int number) => new SocketId(number);

        /// <summary>
        /// Converts a <see cref="SocketId"/> to a number.
        /// </summary>
        /// <param name="socketId">The identifier.</param>
        public static implicit operator int(SocketId socketId) => socketId.id;
    }
}
