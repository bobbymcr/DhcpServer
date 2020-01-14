// <copyright file="MemoryExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides extension methods involving <see cref="Memory{T}"/>.
    /// </summary>
    // For performance reasons, these methods are "duplicated" here and in SpanExtensions.
    // It is faster to index into the Memory buffer than to slice out a Span.
    public static class MemoryExtensions
    {
        /// <summary>
        /// Reads an unsigned 8-bit integer in big-endian format.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public static byte ParseUInt8(this Memory<byte> buffer, int start)
        {
            Span<byte> span = buffer.Span;
            return span[start];
        }

        /// <summary>
        /// Writes an unsigned 8-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        public static void CopyTo(this byte value, Memory<byte> destination, int start)
        {
            Span<byte> span = destination.Span;
            span[start] = value;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer in big-endian format.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public static ushort ParseUInt16(this Memory<byte> buffer, int start)
        {
            Span<byte> span = buffer.Span;
            ushort value = (ushort)(span[start] << 8);
            value |= span[start + 1];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 16-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        public static void CopyTo(this ushort value, Memory<byte> destination, int start)
        {
            Span<byte> span = destination.Span;
            span[start] = (byte)(value >> 8);
            span[start + 1] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer in big-endian format.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public static uint ParseUInt32(this Memory<byte> buffer, int start)
        {
            Span<byte> span = buffer.Span;
            uint value = (uint)(span[start] << 24);
            value |= (uint)(span[start + 1] << 16);
            value |= (uint)(span[start + 2] << 8);
            value |= span[start + 3];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 32-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        public static void CopyTo(this uint value, Memory<byte> destination, int start)
        {
            Span<byte> span = destination.Span;
            span[start] = (byte)(value >> 24);
            span[start + 1] = (byte)(value >> 16);
            span[start + 2] = (byte)(value >> 8);
            span[start + 3] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Reads an <see cref="IPAddressV4"/> in big-endian format.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public static IPAddressV4 ParseIPAddressV4(this Memory<byte> buffer, int start)
        {
            return new IPAddressV4(buffer.ParseUInt32(start));
        }

        /// <summary>
        /// Writes an <see cref="IPAddressV4"/> in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="start">The start index within the buffer.</param>
        public static void CopyTo(this IPAddressV4 value, Memory<byte> destination, int start)
        {
            value.CopyTo(destination.Span.Slice(start, 4));
        }
    }
}
