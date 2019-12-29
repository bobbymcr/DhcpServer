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
            DhcpOptionOverloads overloads = Read(this.options, obj, read);

            if (overloads.HasFlag(DhcpOptionOverloads.File))
            {
                Read(this.file, obj, read);
            }

            if (overloads.HasFlag(DhcpOptionOverloads.SName))
            {
                Read(this.sname, obj, read);
            }
        }

        private static DhcpOptionOverloads Read<T>(Memory<byte> buffer, T obj, Action<DhcpOption, T> read)
        {
            Span<byte> span = buffer.Span;
            int pos = 0;
            int end = span.Length;
            DhcpOptionOverloads overloads = DhcpOptionOverloads.None;
            while (pos < end)
            {
                DhcpOptionTag tag = (DhcpOptionTag)span[pos++];
                byte length;
                switch (tag)
                {
                    case DhcpOptionTag.Pad:
                    case DhcpOptionTag.End:
                        length = 0;
                        break;
                    default:
                        length = span[pos++];
                        break;
                }

                if (tag != DhcpOptionTag.Pad)
                {
                    DhcpOption option = new DhcpOption(tag, buffer.Slice(pos, length));
                    read(option, obj);
                    if (tag == DhcpOptionTag.Overload)
                    {
                        overloads = (DhcpOptionOverloads)option.Data[0];
                    }
                }

                if (tag == DhcpOptionTag.End)
                {
                    break;
                }

                pos += length;
            }

            return overloads;
        }
    }
}
