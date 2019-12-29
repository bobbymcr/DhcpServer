// <copyright file="MessageBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides low-level read/write operations on a pre-allocated buffer.
    /// </summary>
    public readonly struct MessageBuffer
    {
        private readonly Memory<byte> buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBuffer"/> struct.
        /// </summary>
        /// <param name="buffer">The pre-allocated buffer.</param>
        public MessageBuffer(Memory<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Gets a span for the underlying buffer.
        /// </summary>
        public Span<byte> Span => this.buffer.Span;

        /// <summary>
        /// Reads an unsigned 8-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public byte ReadUInt8(int start)
        {
            Span<byte> span = this.buffer.Span;
            return span[start];
        }

        /// <summary>
        /// Writes an unsigned 8-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt8(int start, byte value)
        {
            Span<byte> span = this.buffer.Span;
            span[start] = value;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public ushort ReadUInt16(int start)
        {
            Span<byte> span = this.buffer.Span;
            ushort value = (ushort)(span[start] << 8);
            value |= span[start + 1];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 16-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt16(int start, ushort value)
        {
            Span<byte> span = this.buffer.Span;
            span[start] = (byte)(value >> 8);
            span[start + 1] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public uint ReadUInt32(int start)
        {
            Span<byte> span = this.buffer.Span;
            uint value = (uint)(span[start] << 24);
            value |= (uint)(span[start + 1] << 16);
            value |= (uint)(span[start + 2] << 8);
            value |= span[start + 3];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 32-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt32(int start, uint value)
        {
            Span<byte> span = this.buffer.Span;
            span[start] = (byte)(value >> 24);
            span[start + 1] = (byte)(value >> 16);
            span[start + 2] = (byte)(value >> 8);
            span[start + 3] = (byte)(value & 0xFF);
        }
    }
}
