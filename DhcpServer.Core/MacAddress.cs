// <copyright file="MacAddress.cs" company="Brian Rogers">
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
