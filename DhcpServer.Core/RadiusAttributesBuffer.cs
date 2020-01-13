﻿// <copyright file="RadiusAttributesBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides write access to a buffer containing RADIUS attributes.
    /// </summary>
    public readonly struct RadiusAttributesBuffer
    {
        private readonly DhcpMessageBuffer buffer;
        private readonly Memory<byte> lengthSlice;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusAttributesBuffer"/> struct
        /// and writes the RADIUS attributes sub-option header to the buffer.
        /// </summary>
        /// <param name="buffer">The underlying message buffer.</param>
        public RadiusAttributesBuffer(DhcpMessageBuffer buffer)
        {
            this.buffer = buffer;
            this.buffer.WriteOptionRaw((byte)DhcpRelayAgentSubOptionCode.RadiusAttributes);
            this.lengthSlice = SkipLength(this.buffer);
        }

        /// <summary>
        /// Writes UTF-8 encoding data for the User-Name attribute.
        /// </summary>
        /// <param name="text">The text data.</param>
        public void WriteUserName(ReadOnlySpan<char> text)
        {
            this.buffer.WriteOptionRaw((byte)RadiusAttributeType.UserName);
            Memory<byte> slice = SkipLength(this.buffer);
            byte length = this.buffer.WriteOptionRaw(text, Encoding.UTF8);
            SetLength(slice, (byte)(2 + length));
        }

        /// <summary>
        /// Marks the end of the RADIUS attributes sub-option.
        /// </summary>
        public void End()
        {
            byte length = (byte)(this.lengthSlice.Length - this.buffer.SliceOptionRaw().Length - 1);
            SetLength(this.lengthSlice, length);
        }

        private static Memory<byte> SkipLength(DhcpMessageBuffer buffer)
        {
            Memory<byte> slice = buffer.SliceOptionRaw();
            buffer.WriteOptionRaw(0);
            return slice;
        }

        private static void SetLength(Memory<byte> slice, byte length) => slice.Span[0] = length;
    }
}