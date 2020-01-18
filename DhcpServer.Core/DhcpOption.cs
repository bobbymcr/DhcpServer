// <copyright file="DhcpOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a DHCP option.
    /// </summary>
    public readonly struct DhcpOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpOption"/> struct.
        /// </summary>
        /// <param name="tag">The option tag.</param>
        /// <param name="data">The option data.</param>
        public DhcpOption(DhcpOptionTag tag, Memory<byte> data)
        {
            this.Tag = tag;
            this.Data = data;
        }

        /// <summary>
        /// Gets the option tag.
        /// </summary>
        public DhcpOptionTag Tag { get; }

        /// <summary>
        /// Gets a span for the option data.
        /// </summary>
        public Memory<byte> Data { get; }

        /// <summary>
        /// Gets the sequence of sub-options from this option.
        /// </summary>
        public SubOptionsSequence SubOptions => new SubOptionsSequence(this.Data);

        /// <summary>
        /// Tries to format the current option into the provided span of characters.
        /// </summary>
        /// <param name="destination">When this method returns, this option formatted as a span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
        /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            // Final result should be 'Tag={xxxxxx...}'
            ReadOnlySpan<char> tag = this.Tag.ToString();
            int hexCharLen = 2 * this.Data.Length;
            int requiredLength = 3 + tag.Length + hexCharLen;
            if (destination.Length < requiredLength)
            {
                charsWritten = 0;
                return false;
            }

            tag.CopyTo(destination);
            charsWritten = tag.Length;
            destination[charsWritten++] = '=';
            destination[charsWritten++] = '{';

            for (int i = 0; i < (hexCharLen / 2); ++i)
            {
                byte b = this.Data.Span[i];
                Hex.Format(destination, charsWritten + (2 * i), b);
            }

            charsWritten += hexCharLen;
            destination[charsWritten++] = '}';
            return true;
        }

        /// <summary>
        /// Represents a sequence of DHCP sub-options.
        /// </summary>
        public readonly struct SubOptionsSequence
        {
            private readonly Memory<byte> subOptions;

            /// <summary>
            /// Initializes a new instance of the <see cref="SubOptionsSequence"/> struct.
            /// </summary>
            /// <param name="subOptions">The underlying buffer.</param>
            public SubOptionsSequence(Memory<byte> subOptions)
            {
                this.subOptions = subOptions;
            }

            /// <summary>
            /// Gets an enumerator which reads options in sequential order.
            /// </summary>
            /// <returns>The options enumerator.</returns>
            public Enumerator GetEnumerator() => new Enumerator(this.subOptions);

            /// <summary>
            /// Tries to format the current sub-option sequence into the provided span of characters.
            /// </summary>
            /// <param name="destination">When this method returns, the sub-options formatted as a span of characters.</param>
            /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
            /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
            public bool TryFormat(Span<char> destination, out int charsWritten)
            {
                // Final result should be sub-options separated by newlines, e.g. '<sub1>NL<sub2>NL...'
                charsWritten = 0;
                foreach (DhcpSubOption subOption in this)
                {
                    bool result = subOption.TryFormat(destination.Slice(charsWritten), out int c);
                    charsWritten += c;
                    if (!result || destination.Length < (charsWritten + 2))
                    {
                        return false;
                    }

                    destination[charsWritten++] = '\r';
                    destination[charsWritten++] = '\n';
                }

                return true;
            }
        }

        /// <summary>
        /// An enumerator which reads sub-options in sequential order.
        /// </summary>
        public struct Enumerator
        {
            private readonly Memory<byte> data;

            private DhcpSubOption current;
            private int pos;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="data">The underlying data.</param>
            public Enumerator(Memory<byte> data)
            {
                this.data = data;
                this.current = default;
                this.pos = 0;
            }

            /// <summary>
            /// Gets the <see cref="DhcpSubOption"/> at the current position of the enumerator.
            /// </summary>
            public DhcpSubOption Current => this.current;

            /// <summary>
            /// Advances the enumerator to the next <see cref="DhcpSubOption"/> element.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element;
            /// <c>false</c> if the enumerator has passed the end.</returns>
            public bool MoveNext()
            {
                Span<byte> span = this.data.Span;
                int end = span.Length;
                int i = this.pos;
                if (i < end)
                {
                    byte code = span[i++];
                    byte length = code != 255 ? span[i++] : (byte)(end - i);
                    this.current = new DhcpSubOption(code, this.data.Slice(i, length));
                    this.pos = i + length;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
