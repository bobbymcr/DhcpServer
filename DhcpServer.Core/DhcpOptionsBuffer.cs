// <copyright file="DhcpOptionsBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides read/write access to a fixed size buffer containing DHCP options.
    /// </summary>
    public readonly struct DhcpOptionsBuffer
    {
        private readonly Memory<byte> buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpOptionsBuffer"/> struct.
        /// </summary>
        /// <param name="buffer">The underlying buffer.</param>
        public DhcpOptionsBuffer(Memory<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Reads options in sequential order and passes each one to a user-defined callback.
        /// </summary>
        /// <remarks>The Pad option is processed internally but not passed to the <paramref name="read"/> function.
        /// Reading stops after the first End option is read; this option is passed to the <paramref name="read"/> function.
        /// </remarks>
        /// <typeparam name="T">The user-defined object type.</typeparam>
        /// <param name="obj">A user-defined parameter object.</param>
        /// <param name="read">The user-defined callback.</param>
        public void ReadAll<T>(T obj, Action<DhcpOption, T> read)
        {
            Span<byte> span = this.buffer.Span;
            int pos = 0;
            int end = span.Length;
            while (pos < end)
            {
                DhcpOptionTag tag = (DhcpOptionTag)span[pos++];
                byte length;
                switch (tag)
                {
                    case DhcpOptionTag.End:
                        length = 0;
                        break;
                    default:
                        length = span[pos++];
                        break;
                }

                DhcpOption option = new DhcpOption(tag, this.buffer.Slice(pos, length));
                read(option, obj);

                if (option.Tag == DhcpOptionTag.End)
                {
                    return;
                }

                pos += length;
            }
        }
    }
}
