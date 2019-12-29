﻿// <copyright file="MacAddress.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a MAC address.
    /// </summary>
    public readonly struct MacAddress : IEquatable<MacAddress>
    {
        private readonly ulong value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacAddress"/> struct.
        /// </summary>
        /// <param name="span">The span containing the address bytes.</param>
        public MacAddress(ReadOnlySpan<byte> span)
            : this(span[0], span[1], span[2], span[3], span[4], span[5])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacAddress"/> struct.
        /// </summary>
        /// <param name="b1">The first byte (most significant).</param>
        /// <param name="b2">The second byte.</param>
        /// <param name="b3">The third byte.</param>
        /// <param name="b4">The fourth byte.</param>
        /// <param name="b5">The fifth byte.</param>
        /// <param name="b6">The sixth byte (least significant).</param>
        public MacAddress(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6)
        {
            ulong value = (ulong)b1 << 40;
            value |= (ulong)b2 << 32;
            value |= (ulong)b3 << 24;
            value |= (ulong)b4 << 16;
            value |= (ulong)b5 << 8;
            value |= b6;
            this = new MacAddress(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacAddress"/> struct.
        /// </summary>
        /// <param name="value">The value as an unsigned 64-bit number.</param>
        public MacAddress(ulong value)
        {
            this.value = value;
        }

        /// <summary>
        /// Writes the address to the specified buffer with trailing zero padding.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        public void WriteTo(Span<byte> destination)
        {
            destination[0] = (byte)(this.value >> 40);
            destination[1] = (byte)(this.value >> 32);
            destination[2] = (byte)(this.value >> 24);
            destination[3] = (byte)(this.value >> 16);
            destination[4] = (byte)(this.value >> 8);
            destination[5] = (byte)(this.value & 0xFF);
            destination.Slice(6).Clear();
        }

        /// <inheritdoc/>
        public bool Equals(MacAddress other) => this.value == other.value;

        /// <inheritdoc/>
        public override int GetHashCode() => this.value.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is MacAddress)
            {
                return this.Equals((MacAddress)obj);
            }

            return false;
        }
    }
}
