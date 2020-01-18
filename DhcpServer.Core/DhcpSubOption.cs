// <copyright file="DhcpSubOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a DHCP sub-option.
    /// </summary>
    /// <remarks>The END sub-option (code 255) may carry vendor-specific data
    /// in its <see cref="Data"/> span.</remarks>
    public readonly struct DhcpSubOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpSubOption"/> struct.
        /// </summary>
        /// <param name="code">The sub-option code.</param>
        /// <param name="data">The sub-option data.</param>
        public DhcpSubOption(byte code, Memory<byte> data)
        {
            this.Code = code;
            this.Data = data;
        }

        /// <summary>
        /// Gets the sub-option code.
        /// </summary>
        public byte Code { get; }

        /// <summary>
        /// Gets the sub-option data.
        /// </summary>
        public Memory<byte> Data { get; }

        /// <summary>
        /// Tries to format this sub-option into the provided span of characters.
        /// </summary>
        /// <param name="destination">When this method returns, this sub-option formatted as a span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
        /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            // Final result should be 'xx={xxxx...}'
            charsWritten = 0;
            int hexCharLen = 2 * this.Data.Length;
            int requiredLength = 5 + hexCharLen;
            if (destination.Length < requiredLength)
            {
                return false;
            }

            Hex.Format(destination, 0, this.Code);
            charsWritten = 2;
            destination[charsWritten++] = '=';
            destination[charsWritten++] = '{';
            Span<byte> raw = this.Data.Span;
            for (int i = 0; i < (hexCharLen / 2); ++i)
            {
                byte b = raw[i];
                Hex.Format(destination, charsWritten + (2 * i), b);
            }

            charsWritten += hexCharLen;
            destination[charsWritten++] = '}';
            return true;
        }
    }
}
