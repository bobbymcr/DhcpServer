﻿// <copyright file="MessageBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides low-level read/write operations on a pre-allocated buffer.
    /// </summary>
    public readonly struct MessageBuffer
    {
        private readonly Memory<byte> buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBuffer"/> struct.
        /// </summary>
        /// <param name="buffer">The pre-allocated buffer.</param>
        public MessageBuffer(Memory<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Reads an unsigned 8-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <returns>The value read from the buffer.</returns>
        public byte ReadUInt8(int start) => this.buffer.Span[start];

        /// <summary>
        /// Writes an unsigned 8-bit integer.
        /// </summary>
        /// <param name="start">The start index within the buffer.</param>
        /// <param name="value">The value to write.</param>
        public void WriteUInt8(int start, byte value)
        {
            this.buffer.Span[start] = value;
        }
    }
}
