// <copyright file="DhcpChannelId.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    /// <summary>
    /// An identifier for a <see cref="IDhcpInputChannel"/>, used in operational events.
    /// </summary>
    public readonly struct DhcpChannelId
    {
        private readonly int id;

        private DhcpChannelId(int number)
        {
            this.id = number;
        }

        /// <summary>
        /// Converts a number to a <see cref="DhcpChannelId"/>.
        /// </summary>
        /// <param name="number">The numeric identifier.</param>
        public static implicit operator DhcpChannelId(int number) => new DhcpChannelId(number);

        /// <summary>
        /// Converts a <see cref="DhcpChannelId"/> to a number.
        /// </summary>
        /// <param name="channelId">The identifier.</param>
        public static implicit operator int(DhcpChannelId channelId) => channelId.id;
    }
}
