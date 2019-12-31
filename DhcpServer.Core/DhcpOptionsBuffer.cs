﻿// <copyright file="DhcpOptionsBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides read/write access to a fixed size buffer containing DHCP options.
    /// </summary>
    public readonly struct DhcpOptionsBuffer
    {
        private readonly Memory<byte> options;
        private readonly Memory<byte> file;
        private readonly Memory<byte> sname;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpOptionsBuffer"/> struct.
        /// </summary>
        /// <param name="options">The options buffer.</param>
        /// <param name="file">The 'file' buffer (for option overload).</param>
        /// <param name="sname">The 'sname' buffer (for option overload).</param>
        public DhcpOptionsBuffer(Memory<byte> options, Memory<byte> file, Memory<byte> sname)
        {
            this.options = options;
            this.file = file;
            this.sname = sname;
        }

        /// <summary>
        /// Gets an enumerator which reads options in sequential order.
        /// </summary>
        /// <returns>The options enumerator.</returns>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Writes an option header to the buffer and slices out a data segment.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="tag">The option tag.</param>
        /// <param name="length">The length of the option data.</param>
        /// <returns>The sliced option.</returns>
        public DhcpOption Slice(int start, DhcpOptionTag tag, byte length)
        {
            return new DhcpOption(tag, this.SliceInner(start, (byte)tag, length));
        }

        /// <summary>
        /// Writes a sub-option header to the buffer and slices out a data segment.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="code">The sub-option code.</param>
        /// <param name="length">The length of the option data.</param>
        /// <returns>The sliced sub-option.</returns>
        public DhcpSubOption SliceSub(int start, byte code, byte length)
        {
            return new DhcpSubOption(code, this.SliceInner(start, code, length));
        }

        /// <summary>
        /// Writes an option header followed by character data to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="tag">The option tag.</param>
        /// <param name="chars">The character buffer.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The sliced option.</returns>
        public DhcpOption Write(int start, DhcpOptionTag tag, ReadOnlySpan<char> chars, Encoding encoding)
        {
            return new DhcpOption(tag, this.WriteInner(start, (byte)tag, chars, encoding));
        }

        /// <summary>
        /// Writes a sub-option header followed by character data to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="code">The sub-option code.</param>
        /// <param name="chars">The character buffer.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The sliced option.</returns>
        public DhcpSubOption WriteSub(int start, byte code, ReadOnlySpan<char> chars, Encoding encoding)
        {
            return new DhcpSubOption(code, this.WriteInner(start, code, chars, encoding));
        }

        /// <summary>
        /// Writes padding (zero-value) bytes to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="length">The number of padding bytes.</param>
        public void Pad(int start, byte length) => this.options.Span.Slice(start, length).Clear();

        /// <summary>
        /// Writes an option end marker to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        public void End(int start)
        {
            this.options.Span[start] = (byte)DhcpOptionTag.End;
        }

        private Memory<byte> SliceInner(int start, byte code, byte length)
        {
            Span<byte> header = this.options.Span;
            header[start] = code;
            header[start + 1] = length;
            return this.options.Slice(start + 2, length);
        }

        private Memory<byte> WriteInner(int start, byte code, ReadOnlySpan<char> chars, Encoding encoding)
        {
            Span<byte> option = this.options.Span;
            option[start] = (byte)code;
            int length = encoding.GetBytes(chars, option.Slice(start + 2));
            option[start + 1] = (byte)length;
            return this.options.Slice(start + 2, length);
        }

        /// <summary>
        /// An enumerator which reads options in sequential order.
        /// </summary>
        public struct Enumerator
        {
            private readonly DhcpOptionsBuffer parent;

            private DhcpOption current;
            private int pos;
            private DhcpOptionOverloads overloads;
            private Memory<byte> buffer;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="parent">The parent buffer.</param>
            public Enumerator(DhcpOptionsBuffer parent)
            {
                this.parent = parent;
                this.current = default;
                this.pos = 0;
                this.overloads = DhcpOptionOverloads.None;
                this.buffer = this.parent.options;
            }

            /// <summary>
            /// Gets the <see cref="DhcpOption"/> at the current position of the enumerator.
            /// </summary>
            public DhcpOption Current => this.current;

            /// <summary>
            /// Advances the enumerator to the next <see cref="DhcpOption"/> element.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element;
            /// <c>false</c> if the enumerator has passed the end.</returns>
            public bool MoveNext()
            {
                do
                {
                    int end = this.buffer.Length;
                    if (this.pos < end)
                    {
                        if (this.NextOption(end))
                        {
                            return true;
                        }
                    }
                    else if (!this.NextBuffer())
                    {
                        return false;
                    }
                }
                while (true);
            }

            private bool NextOption(int end)
            {
                int i = this.pos;
                Span<byte> span = this.buffer.Span;
                DhcpOptionTag tag = (DhcpOptionTag)span[i++];
                byte length;
                switch (tag)
                {
                    case DhcpOptionTag.Pad:
                        this.pos = i;
                        return false;
                    case DhcpOptionTag.End:
                        length = 0;
                        break;
                    default:
                        length = span[i++];
                        break;
                }

                this.current = new DhcpOption(tag, this.buffer.Slice(i, length));
                switch (tag)
                {
                    case DhcpOptionTag.Overload:
                        this.overloads = (DhcpOptionOverloads)span[i];
                        break;
                    case DhcpOptionTag.End:
                        this.pos = end;
                        return true;
                }

                this.pos = i + length;
                return true;
            }

            private bool NextBuffer()
            {
                switch (this.overloads)
                {
                    case DhcpOptionOverloads.None:
                        return false;
                    case DhcpOptionOverloads.File:
                        this.overloads = DhcpOptionOverloads.None;
                        this.buffer = this.parent.file;
                        break;
                    case DhcpOptionOverloads.SName:
                        this.overloads = DhcpOptionOverloads.None;
                        this.buffer = this.parent.sname;
                        break;
                    default:
                        this.overloads = DhcpOptionOverloads.SName;
                        this.buffer = this.parent.file;
                        break;
                }

                this.pos = 0;
                return true;
            }
        }
    }
}
