// <copyright file="DhcpMessageBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Provides read/write access to a fixed size buffer containing a DHCP message.
    /// </summary>
    public sealed class DhcpMessageBuffer
    {
        private readonly MessageBuffer buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpMessageBuffer"/> class.
        /// </summary>
        /// <param name="buffer">The underlying buffer.</param>
        public DhcpMessageBuffer(Memory<byte> buffer)
        {
            this.buffer = new MessageBuffer(buffer);
        }

        /// <summary>
        /// Gets a span for the underlying buffer.
        /// </summary>
        public Span<byte> Span => this.buffer.Span;

        /// <summary>
        /// Gets or sets the message opcode.
        /// </summary>
        public DhcpOpcode Opcode { get; set; }

        /// <summary>
        /// Gets or sets the hardware address type.
        /// </summary>
        public DhcpHardwareAddressType HardwareAddressType { get; set; }

        /// <summary>
        /// Gets or sets the hardware address length.
        /// </summary>
        public byte HardwareAddressLength { get; set; }

        /// <summary>
        /// Loads and parses message data from the underlying buffer.
        /// </summary>
        /// <param name="length">The length of the message.</param>
        public void Load(int length)
        {
            int current = 0;
            this.Opcode = (DhcpOpcode)this.buffer.ReadUInt8(current++);
            this.HardwareAddressType = (DhcpHardwareAddressType)this.buffer.ReadUInt8(current++);
            this.HardwareAddressLength = this.buffer.ReadUInt8(current++);
        }
    }
}
