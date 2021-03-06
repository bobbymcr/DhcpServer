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
        /// Copies the address bytes into a destination span with trailing zero padding.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        public void CopyTo(Span<byte> destination)
        {
            destination[0] = (byte)(this.value >> 40);
            destination[1] = (byte)(this.value >> 32);
            destination[2] = (byte)(this.value >> 24);
            destination[3] = (byte)(this.value >> 16);
            destination[4] = (byte)(this.value >> 8);
            destination[5] = (byte)(this.value & 0xFF);
            destination.Slice(6).Clear();
        }

        /// <summary>
        /// Tries to format the value of the address into the provided span of characters.
        /// </summary>
        /// <param name="destination">When this method returns, this instance's value formatted as a
        /// span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters that were
        /// written in <paramref name="destination"/>.</param>
        /// <param name="format">The format specifier ("D" or "N"); if empty, "D" is used.</param>
        /// <remarks>
        /// <list type="table">
        ///   <listheader>
        ///     <term>Specifier</term>
        ///     <description>Format of return value</description>
        ///   </listheader>
        ///   <item>
        ///     <term><c>N</c></term>
        ///     <description>12 hexadecimal digits: 000000000000</description>
        ///   </item>
        ///   <item>
        ///     <term><c>D</c></term>
        ///     <description>12 hexadecimal digits separated by hyphens: 00-00-00-00-00-00</description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <returns><c>true </c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default)
        {
            if (format == "N")
            {
                return this.TryFormatNoHyphens(destination, out charsWritten);
            }

            return this.TryFormatHyphens(destination, out charsWritten);
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

        private bool TryFormatHyphens(Span<char> destination, out int charsWritten)
        {
            if (destination.Length < 17)
            {
                charsWritten = 0;
                return false;
            }

            Hex.Format(destination, 0, (byte)(this.value >> 40));
            destination[2] = '-';
            Hex.Format(destination, 3, (byte)(this.value >> 32));
            destination[5] = '-';
            Hex.Format(destination, 6, (byte)(this.value >> 24));
            destination[8] = '-';
            Hex.Format(destination, 9, (byte)(this.value >> 16));
            destination[11] = '-';
            Hex.Format(destination, 12, (byte)(this.value >> 8));
            destination[14] = '-';
            Hex.Format(destination, 15, (byte)(this.value & 0xFF));
            charsWritten = 17;
            return true;
        }

        private bool TryFormatNoHyphens(Span<char> destination, out int charsWritten)
        {
            if (destination.Length < 12)
            {
                charsWritten = 0;
                return false;
            }

            Hex.Format(destination, 0, (byte)(this.value >> 40));
            Hex.Format(destination, 2, (byte)(this.value >> 32));
            Hex.Format(destination, 4, (byte)(this.value >> 24));
            Hex.Format(destination, 6, (byte)(this.value >> 16));
            Hex.Format(destination, 8, (byte)(this.value >> 8));
            Hex.Format(destination, 10, (byte)(this.value & 0xFF));
            charsWritten = 12;
            return true;
        }
    }
}
