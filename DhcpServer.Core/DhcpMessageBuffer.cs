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
        /// Gets or sets the hop count.
        /// </summary>
        public byte Hops { get; set; }

        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public uint TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the seconds elapsed since client boot.
        /// </summary>
        public ushort Seconds { get; set; }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public DhcpFlags Flags { get; set; }

        /// <summary>
        /// Loads and parses message data from the underlying buffer.
        /// </summary>
        /// <param name="length">The length of the message.</param>
        public void Load(int length)
        {
            int current = 0;
            this.Opcode = (DhcpOpcode)this.NextUInt8(ref current);
            this.HardwareAddressType = (DhcpHardwareAddressType)this.NextUInt8(ref current);
            this.HardwareAddressLength = this.NextUInt8(ref current);
            this.Hops = this.NextUInt8(ref current);
            this.TransactionId = this.NextUInt32(ref current);
            this.Seconds = this.NextUInt16(ref current);
            this.Flags = (DhcpFlags)this.NextUInt16(ref current);
        }

        private byte NextUInt8(ref int current) => this.buffer.ReadUInt8(current++);

        private ushort NextUInt16(ref int current)
        {
            ushort value = this.buffer.ReadUInt16(current);
            current += 2;
            return value;
        }

        private uint NextUInt32(ref int current)
        {
            uint value = this.buffer.ReadUInt32(current);
            current += 4;
            return value;
        }
    }
}
