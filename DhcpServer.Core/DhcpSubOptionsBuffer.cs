// <copyright file="DhcpSubOptionsBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides write access to a buffer containing generic sub-options.
    /// </summary>
    public readonly struct DhcpSubOptionsBuffer
    {
        private readonly DhcpMessageBuffer buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpSubOptionsBuffer"/> struct
        /// and writes the specified option header to the buffer.
        /// </summary>
        /// <param name="buffer">The underlying message buffer.</param>
        /// <param name="tag">The option tag.</param>
        public DhcpSubOptionsBuffer(DhcpMessageBuffer buffer, DhcpOptionTag tag)
        {
            this.buffer = buffer;
            this.buffer.WriteContainerOptionHeader(tag);
        }

        /// <summary>
        /// Writes a raw data item.
        /// </summary>
        /// <param name="code">The data code.</param>
        /// <param name="data">The data buffer.</param>
        public void WriteDataItem(byte code, ReadOnlySpan<byte> data)
        {
            var item = this.buffer.WriteSubOptionHeader(code, (byte)data.Length);
            data.CopyTo(item.Data);
        }

        /// <summary>
        /// Marks the end of the container option.
        /// </summary>
        public void End() => this.buffer.EndContainerOption();
    }
}
