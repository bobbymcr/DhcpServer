// <copyright file="IPAddressV4.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing an IPv4 address.
    /// </summary>
    public readonly struct IPAddressV4 : IEquatable<IPAddressV4>
    {
        private readonly uint value;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressV4"/> struct.
        /// </summary>
        /// <param name="b1">The first byte (most significant).</param>
        /// <param name="b2">The second byte.</param>
        /// <param name="b3">The third byte.</param>
        /// <param name="b4">The fourth byte (least significant).</param>
        public IPAddressV4(byte b1, byte b2, byte b3, byte b4)
        {
            uint value = (uint)(b1 << 24);
            value |= (uint)(b2 << 16);
            value |= (uint)(b3 << 8);
            value |= b4;
            this = new IPAddressV4(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressV4"/> struct.
        /// </summary>
        /// <param name="value">The value as an unsigned 32-bit number.</param>
        public IPAddressV4(uint value)
        {
            this.value = value;
        }

        /// <summary>
        /// Writes the address to the specified buffer.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        public void WriteTo(Span<byte> destination)
        {
            destination[0] = (byte)(this.value >> 24);
            destination[1] = (byte)(this.value >> 16);
            destination[2] = (byte)(this.value >> 8);
            destination[3] = (byte)(this.value & 0xFF);
        }

        /// <inheritdoc/>
        public bool Equals(IPAddressV4 other) => this.value == other.value;

        /// <inheritdoc/>
        public override int GetHashCode() => this.value.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is IPAddressV4)
            {
                return this.Equals((IPAddressV4)obj);
            }

            return false;
        }
    }
}
