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
        /// <summary>
        /// Provides the loopback address.
        /// </summary>
        public static readonly IPAddressV4 Loopback = new IPAddressV4(127, 0, 0, 1);

        private static readonly byte[] DigitCount = new byte[256]
        {
            // 01 02 03...
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 33...
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 65...
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 97...
            2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 129...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 161...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 193...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 225...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        };

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
        /// <param name="value">The value as an unsigned 32-bit integer.</param>
        public IPAddressV4(uint value)
        {
            this.value = value;
        }

        /// <summary>
        /// Converts the <see cref="IPAddressV4"/> to an unsigned 32-bit integer in little-endian format.
        /// </summary>
        /// <param name="address">The address.</param>
        public static explicit operator uint(IPAddressV4 address)
        {
            uint v = address.value;
            uint r = (v & 0x000000FF) << 24;
            r |= (v & 0x00FF0000) >> 8;
            r |= (v & 0x00000FF00) << 8;
            r |= (v & 0xFF000000) >> 24;
            return r;
        }

        /// <summary>
        /// Copies the address bytes into a destination span in big-endian format.
        /// </summary>
        /// <param name="destination">The destination buffer.</param>
        public void CopyTo(Span<byte> destination) => this.value.CopyTo(destination);

        /// <summary>
        /// Tries to format the current IP address into the provided span.
        /// </summary>
        /// <param name="destination">When this method returns, the IP address as a span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters written into the span.</param>
        /// <returns><c>true </c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            byte b0 = (byte)(this.value >> 24);
            byte b1 = (byte)(this.value >> 16);
            byte b2 = (byte)(this.value >> 8);
            byte b3 = (byte)(this.value & 0xFF);
            int requiredLength = DigitCount[b0] + DigitCount[b1] + DigitCount[b2] + DigitCount[b3] + 3;
            if (destination.Length < requiredLength)
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = 0;
            WriteUInt8(destination, ref charsWritten, b0);
            destination[charsWritten++] = '.';
            WriteUInt8(destination, ref charsWritten, b1);
            destination[charsWritten++] = '.';
            WriteUInt8(destination, ref charsWritten, b2);
            destination[charsWritten++] = '.';
            WriteUInt8(destination, ref charsWritten, b3);
            return true;
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

        private static void WriteUInt8(Span<char> destination, ref int start, byte value)
        {
            if (value > 99)
            {
                Base10.WriteDigits3(destination, start, value);
                start += 3;
            }
            else if (value > 9)
            {
                Base10.WriteDigits2(destination, start, value);
                start += 2;
            }
            else
            {
                Base10.WriteDigit(destination, start, value);
                start += 1;
            }
        }
    }
}
