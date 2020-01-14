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
    }
}
