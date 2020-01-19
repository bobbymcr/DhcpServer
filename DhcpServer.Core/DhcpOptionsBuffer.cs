// <copyright file="DhcpOptionsBuffer.cs" company="Brian Rogers">
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
        /// Slices out a data segment from the starting index to the end of the buffer.
        /// </summary>
        /// <param name="start">The starting index.</param>
        /// <returns>The sliced data segment.</returns>
        public Memory<byte> Slice(int start) => this.options.Slice(start);

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
        /// Writes raw data to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="value">The data value.</param>
        public void WriteRaw(int start, byte value) => value.CopyTo(this.options, start);

        /// <summary>
        /// Writes raw data to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="value">The data value.</param>
        public void WriteRaw(int start, uint value) => value.CopyTo(this.options, start);

        /// <summary>
        /// Writes raw data to the buffer and advances the cursor.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        /// <param name="chars">The character data.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The number of bytes written.</returns>
        public byte WriteRaw(int start, ReadOnlySpan<char> chars, Encoding encoding)
        {
            return (byte)encoding.GetBytes(chars, this.options.Span.Slice(start));
        }

        /// <summary>
        /// Writes an option end marker to the buffer.
        /// </summary>
        /// <param name="start">The starting index in the buffer.</param>
        public void End(int start)
        {
            ((byte)DhcpOptionTag.End).CopyTo(this.options, start);
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
            option[start] = code;
            byte length = this.WriteRaw(start + 2, chars, encoding);
            option[start + 1] = length;
            return this.options.Slice(start + 2, length);
        }

        /// <summary>
        /// An enumerator which reads options in sequential order.
        /// </summary>
        public struct Enumerator
        {
            private readonly DhcpOptionsBuffer parent;

            private DhcpOption current;
            private ushort pos;
            private bool overloadSeen;
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
                this.overloadSeen = false;
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
                    ushort end = (ushort)this.buffer.Length;
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

            private bool NextOption(ushort end)
            {
                ushort i = this.pos;
                Span<byte> span = this.buffer.Span;
                DhcpOptionTag tag = (DhcpOptionTag)span[i++];
                int length;
                switch (tag)
                {
                    case DhcpOptionTag.Pad:
                        this.pos = i;
                        return false;
                    case DhcpOptionTag.End:
                        length = 0;
                        break;
                    default:
                        if (i++ != end)
                        {
                            length = span[i - 1];
                        }
                        else
                        {
                            // Corrupt option -- handled below
                            length = 0;
                        }

                        break;
                }

                Memory<byte> data;
                if ((i + length) <= end)
                {
                    data = this.buffer.Slice(i, length);
                }
                else
                {
                    // Corrupt option; return raw payload wrapped in an 'End' option
                    tag = DhcpOptionTag.End;
                    data = this.buffer.Slice(i - 2);
                }

                this.current = new DhcpOption(tag, data);
                switch (tag)
                {
                    case DhcpOptionTag.Overload:
                        // Assume invalid unless length is exactly 1
                        if (length == 1)
                        {
                            this.SetOverloads((DhcpOptionOverloads)span[i]);
                        }

                        break;
                    case DhcpOptionTag.End:
                        this.pos = end;
                        return true;
                }

                this.pos = (ushort)(i + length);
                return true;
            }

            private void SetOverloads(DhcpOptionOverloads value)
            {
                // Only process a valid overload value, and only set once!
                // Otherwise, we may never finish enumerating
                if (!this.overloadSeen && ((value | DhcpOptionOverloads.Both) == DhcpOptionOverloads.Both))
                {
                    this.overloads = value;
                    this.overloadSeen = true;
                }
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
