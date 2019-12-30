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

        /// <summary>
        /// Reads sub-options in sequential order and passes each one to a user-defined callback.
        /// </summary>
        /// <typeparam name="T">The user-defined object type.</typeparam>
        /// <param name="obj">A user-defined parameter object.</param>
        /// <param name="read">The user-defined callback.</param>
        public void ReadSubOptions<T>(T obj, Action<DhcpSubOption, T> read)
        {
            Span<byte> span = this.Data;
            int pos = 0;
            int end = span.Length;
            while (pos < end)
            {
                byte code = span[pos++];
                byte length = span[pos++];
                DhcpSubOption subOption = new DhcpSubOption(code, this.data.Slice(pos, length));
                read(subOption, obj);
                pos += length;
            }
        }
    }
}
