﻿// <copyright file="DhcpRelayAgentInformationSubOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Represents a DHCP relay agent information sub-option.
    /// </summary>
    public readonly struct DhcpRelayAgentInformationSubOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpRelayAgentInformationSubOption"/> struct.
        /// </summary>
        /// <param name="code">The sub-option code.</param>
        /// <param name="data">The sub-option data.</param>
        public DhcpRelayAgentInformationSubOption(DhcpRelayAgentSubOptionCode code, Memory<byte> data)
        {
            this.Code = code;
            this.Data = data;
        }

        /// <summary>
        /// Gets the sub-option code.
        /// </summary>
        public DhcpRelayAgentSubOptionCode Code { get; }

        /// <summary>
        /// Gets the sub-option data.
        /// </summary>
        public Memory<byte> Data { get; }

        /// <summary>
        /// Tries to format the current relay agent information sub-option into the provided span of characters.
        /// </summary>
        /// <param name="destination">When this method returns, the relay agent information sub-option formatted as a span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
        /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            // Final result should be 'Code={xxxx...}'
            charsWritten = 0;
            ReadOnlySpan<char> code = this.Code.ToString();
            int hexCharLen = 2 * this.Data.Length;
            int requiredLength = code.Length + 3 + hexCharLen;
            if (destination.Length < requiredLength)
            {
                return false;
            }

            code.CopyTo(destination);
            charsWritten += code.Length;
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

        /// <summary>
        /// An enumerator which reads relay agent information sub-options in sequential order.
        /// </summary>
        public struct Enumerator
        {
            private readonly Memory<byte> data;

            private DhcpRelayAgentInformationSubOption current;
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
            /// Gets the <see cref="DhcpRelayAgentInformationSubOption"/> at the current position of the enumerator.
            /// </summary>
            public DhcpRelayAgentInformationSubOption Current => this.current;

            /// <summary>
            /// Advances the enumerator to the next <see cref="DhcpRelayAgentInformationSubOption"/> element.
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
                    DhcpRelayAgentSubOptionCode code = (DhcpRelayAgentSubOptionCode)span[i++];
                    byte length = span[i++];
                    this.current = new DhcpRelayAgentInformationSubOption(code, this.data.Slice(i, length));
                    this.pos = i + length;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Represents a sequence of relay agent information sub-options.
        /// </summary>
        public readonly struct Sequence
        {
            private readonly DhcpOption option;

            /// <summary>
            /// Initializes a new instance of the <see cref="Sequence"/> struct.
            /// </summary>
            /// <param name="option">The relay agent information DHCP option.</param>
            public Sequence(DhcpOption option)
            {
                this.option = option;
            }

            /// <summary>
            /// Gets an enumerator which reads relay agent information sub-options in sequential order.
            /// </summary>
            /// <returns>The sub-options enumerator.</returns>
            public Enumerator GetEnumerator() => new Enumerator(this.option.Data);

            /// <summary>
            /// Tries to format the current relay agent information sequence into the provided span of characters.
            /// </summary>
            /// <param name="destination">When this method returns, the relay agent information formatted as a span of characters.</param>
            /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
            /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
            public bool TryFormat(Span<char> destination, out int charsWritten)
            {
                // Final result should be relay agent information sub-options separated by newlines, e.g. '<sub1>NL<sub2>NL...'
                charsWritten = 0;
                foreach (DhcpRelayAgentInformationSubOption subOption in this)
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
    }
}