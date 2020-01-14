// <copyright file="SpanExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides extension methods involving <see cref="Span{T}"/>.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Reads an unsigned 8-bit integer in big-endian format.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <returns>The value read from the span.</returns>
        public static byte ParseUInt8(this Span<byte> span)
        {
            return span[0];
        }

        /// <summary>
        /// Writes an unsigned 8-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination span.</param>
        public static void CopyTo(this byte value, Span<byte> destination)
        {
            destination[0] = value;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer in big-endian format.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <returns>The value read from the span.</returns>
        public static ushort ParseUInt16(this Span<byte> span)
        {
            ushort value = (ushort)(span[0] << 8);
            value |= span[1];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 16-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination span.</param>
        public static void CopyTo(this ushort value, Span<byte> destination)
        {
            destination[0] = (byte)(value >> 8);
            destination[1] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer in big-endian format.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <returns>The value read from the span.</returns>
        public static uint ParseUInt32(this Span<byte> span)
        {
            uint value = (uint)(span[0] << 24);
            value |= (uint)(span[1] << 16);
            value |= (uint)(span[2] << 8);
            value |= span[3];
            return value;
        }

        /// <summary>
        /// Writes an unsigned 32-bit integer in big-endian format.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="destination">The destination span.</param>
        public static void CopyTo(this uint value, Span<byte> destination)
        {
            destination[0] = (byte)(value >> 24);
            destination[1] = (byte)(value >> 16);
            destination[2] = (byte)(value >> 8);
            destination[3] = (byte)(value & 0xFF);
        }

        /// <summary>
        /// Reads an <see cref="IPAddressV4"/> in big-endian format.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <returns>The value read from the span.</returns>
        public static IPAddressV4 ParseIPAddressV4(this Span<byte> span)
        {
            return new IPAddressV4(span.ParseUInt32());
        }
    }
}
