// <copyright file="DhcpOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a DHCP option.
    /// </summary>
    public readonly struct DhcpOption
    {
        private readonly Memory<byte> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpOption"/> struct.
        /// </summary>
        /// <param name="tag">The option tag.</param>
        /// <param name="data">The option data.</param>
        public DhcpOption(DhcpOptionTag tag, Memory<byte> data)
        {
            this.Tag = tag;
            this.data = data;
        }

        /// <summary>
        /// Gets the option tag.
        /// </summary>
        public DhcpOptionTag Tag { get; }

        /// <summary>
        /// Gets a span for the option data.
        /// </summary>
        public Span<byte> Data => this.data.Span;
    }
}
