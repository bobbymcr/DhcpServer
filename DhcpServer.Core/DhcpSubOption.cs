// <copyright file="DhcpSubOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a DHCP sub-option.
    /// </summary>
    public readonly struct DhcpSubOption
    {
        private readonly Memory<byte> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpSubOption"/> struct.
        /// </summary>
        /// <param name="code">The sub-option code.</param>
        /// <param name="data">The sub-option data.</param>
        public DhcpSubOption(byte code, Memory<byte> data)
        {
            this.Code = code;
            this.data = data;
        }

        /// <summary>
        /// Gets the sub-option code.
        /// </summary>
        public byte Code { get; }

        /// <summary>
        /// Gets a span for the sub-option data.
        /// </summary>
        public Span<byte> Data => this.data.Span;
    }
}
