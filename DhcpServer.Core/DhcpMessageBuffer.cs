// <copyright file="DhcpMessageBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides read/write access to a fixed size buffer containing a DHCP message.
    /// </summary>
    public sealed class DhcpMessageBuffer
    {
        private const int HeaderLength = 240;
        private const int ServerHostNameStart = 44;
        private const int ServerHostNameLength = 64;
        private const int BootFileNameStart = 108;
        private const int BootFileNameLength = 128;

        private readonly MessageBuffer buffer;

        private DhcpOptionsBuffer options;
        private int nextOption;
        private int containerStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpMessageBuffer"/> class.
        /// </summary>
        /// <param name="buffer">The underlying buffer.</param>
        /// <exception cref="ArgumentOutOfRangeException">The buffer is too small to hold a valid message.</exception>
        public DhcpMessageBuffer(Memory<byte> buffer)
        {
            this.buffer = new MessageBuffer(buffer);
            if (!this.SetOptions(buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(buffer));
            }
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
        /// Gets or sets the client-specified IP address.
        /// </summary>
        public IPAddressV4 ClientIPAddress { get; set; }

        /// <summary>
        /// Gets or sets the IP address assigned by the server to the client.
        /// </summary>
        public IPAddressV4 YourIPAddress { get; set; }

        /// <summary>
        /// Gets or sets the server's IP address.
        /// </summary>
        public IPAddressV4 ServerIPAddress { get; set; }

        /// <summary>
        /// Gets or sets the gateway IP address.
        /// </summary>
        public IPAddressV4 GatewayIPAddress { get; set; }

        /// <summary>
        /// Gets a span for the client hardware address bytes.
        /// </summary>
        public Span<byte> ClientHardwareAddress => this.Span.Slice(28, this.HardwareAddressLength);

        /// <summary>
        /// Gets a span for the server host name bytes.
        /// </summary>
        public Span<byte> ServerHostName => this.Span.Slice(ServerHostNameStart, ServerHostNameLength);

        /// <summary>
        /// Gets a span for the boot file name bytes.
        /// </summary>
        public Span<byte> BootFileName => this.Span.Slice(BootFileNameStart, BootFileNameLength);

        /// <summary>
        /// Gets or sets the magic cookie.
        /// </summary>
        public MagicCookie MagicCookie { get; set; }

        /// <summary>
        /// Gets the sequence of options from the DHCP message.
        /// </summary>
        /// <remarks>The Pad option is processed internally but not returned.
        /// Reading stops after the End option is read; this option is returned.
        /// </remarks>
        public OptionsSequence Options => new OptionsSequence(this.options);

        /// <summary>
        /// Loads and parses message data from the underlying buffer.
        /// </summary>
        /// <param name="length">The length of the message.</param>
        /// <returns><c>true</c> if the length is enough to contain a valid message, <c>false</c> otherwise.</returns>
        public bool Load(int length)
        {
            if (!this.SetOptions(length))
            {
                return false;
            }

            this.Opcode = (DhcpOpcode)this.buffer.ReadUInt8(0);
            this.HardwareAddressType = (DhcpHardwareAddressType)this.buffer.ReadUInt8(1);
            this.HardwareAddressLength = this.buffer.ReadUInt8(2);
            this.Hops = this.buffer.ReadUInt8(3);
            this.TransactionId = this.buffer.ReadUInt32(4);
            this.Seconds = this.buffer.ReadUInt16(8);
            this.Flags = (DhcpFlags)this.buffer.ReadUInt16(10);
            this.ClientIPAddress = new IPAddressV4(this.buffer.ReadUInt32(12));
            this.YourIPAddress = new IPAddressV4(this.buffer.ReadUInt32(16));
            this.ServerIPAddress = new IPAddressV4(this.buffer.ReadUInt32(20));
            this.GatewayIPAddress = new IPAddressV4(this.buffer.ReadUInt32(24));
            this.MagicCookie = (MagicCookie)this.buffer.ReadUInt32(236);
            return true;
        }

        /// <summary>
        /// Saves message data to the underlying buffer and resets the cursor.
        /// </summary>
        /// <returns>The total message length.</returns>
        public int Save()
        {
            this.buffer.WriteUInt8(0, (byte)this.Opcode);
            this.buffer.WriteUInt8(1, (byte)this.HardwareAddressType);
            this.buffer.WriteUInt8(2, this.HardwareAddressLength);
            this.buffer.WriteUInt8(3, this.Hops);
            this.buffer.WriteUInt32(4, this.TransactionId);
            this.buffer.WriteUInt16(8, this.Seconds);
            this.buffer.WriteUInt16(10, (ushort)this.Flags);
            this.ClientIPAddress.WriteTo(this.buffer.Span.Slice(12, 4));
            this.YourIPAddress.WriteTo(this.buffer.Span.Slice(16, 4));
            this.ServerIPAddress.WriteTo(this.buffer.Span.Slice(20, 4));
            this.GatewayIPAddress.WriteTo(this.buffer.Span.Slice(24, 4));
            this.buffer.WriteUInt32(236, (uint)this.MagicCookie);
            int totalLength = this.nextOption + HeaderLength;
            this.nextOption = 0;
            return totalLength;
        }

        /// <summary>
        /// Writes a container option header to the buffer with a variable sized data segment and advances the cursor.
        /// </summary>
        /// <remarks>At least one call to either <see cref="WriteSubOptionHeader(byte, byte)"/> or
        /// <see cref="WriteSubOption(byte, ReadOnlySpan{char}, Encoding)"/> must be made after this.
        /// The container option must be terminated by an <see cref="EndContainerOption"/> call.</remarks>
        /// <param name="tag">The container option tag.</param>
        public void WriteContainerOptionHeader(DhcpOptionTag tag)
        {
            this.options.Slice(this.nextOption, tag, 0);
            this.containerStart = this.nextOption;
            this.nextOption += 2;
        }

        /// <summary>
        /// Writes a sub-option header to the buffer, slices out a data segment, and advances the cursor.
        /// </summary>
        /// <remarks>This must be called after <see cref="WriteContainerOptionHeader(DhcpOptionTag)"/>.</remarks>
        /// <param name="code">The sub-option code.</param>
        /// <param name="length">The sub-option length.</param>
        /// <returns>The sliced sub-option.</returns>
        public DhcpSubOption WriteSubOptionHeader(byte code, byte length)
        {
            DhcpSubOption subOption = this.options.SliceSub(this.nextOption, code, length);
            this.nextOption += 2 + length;
            return subOption;
        }

        /// <summary>
        /// Writes a sub-option header followed by character data to the buffer and advances the cursor.
        /// </summary>
        /// <remarks>This must be called after <see cref="WriteContainerOptionHeader(DhcpOptionTag)"/>.</remarks>
        /// <param name="code">The sub-option code.</param>
        /// <param name="chars">The character buffer.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The sliced option.</returns>
        public DhcpSubOption WriteSubOption(byte code, ReadOnlySpan<char> chars, Encoding encoding)
        {
            DhcpSubOption option = this.options.WriteSub(this.nextOption, code, chars, encoding);
            this.nextOption += 2 + option.Data.Length;
            return option;
        }

        /// <summary>
        /// Marks the end of the current container option and updates the length.
        /// </summary>
        /// <remarks>At least one call to either <see cref="WriteSubOptionHeader(byte, byte)"/> or
        /// <see cref="WriteSubOption(byte, ReadOnlySpan{char}, Encoding)"/> must be made before this.
        /// </remarks>
        public void EndContainerOption()
        {
            this.Span[HeaderLength + this.containerStart + 1] = (byte)(this.nextOption - this.containerStart - 2);
        }

        /// <summary>
        /// Writes an option header to the buffer, slices out a data segment, and advances the cursor.
        /// </summary>
        /// <param name="tag">The option tag.</param>
        /// <param name="length">The length of the option data.</param>
        /// <returns>The sliced option.</returns>
        public DhcpOption WriteOptionHeader(DhcpOptionTag tag, byte length)
        {
            DhcpOption option = this.options.Slice(this.nextOption, tag, length);
            this.nextOption += 2 + length;
            return option;
        }

        /// <summary>
        /// Writes an option header followed by character data to the buffer and advances the cursor.
        /// </summary>
        /// <param name="tag">The option tag.</param>
        /// <param name="chars">The character buffer.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The sliced option.</returns>
        public DhcpOption WriteOption(DhcpOptionTag tag, ReadOnlySpan<char> chars, Encoding encoding)
        {
            DhcpOption option = this.options.Write(this.nextOption, tag, chars, encoding);
            this.nextOption += 2 + option.Data.Length;
            return option;
        }

        /// <summary>
        /// Writes option padding (zero-value) bytes and advances the cursor.
        /// </summary>
        /// <param name="length">The number of padding bytes.</param>
        public void WritePadding(byte length)
        {
            this.options.Pad(this.nextOption, length);
            this.nextOption += length;
        }

        /// <summary>
        /// Writes an option end marker and advances the cursor.
        /// </summary>
        public void WriteEndOption() => this.options.End(this.nextOption++);

        private bool SetOptions(int length)
        {
            if (length >= HeaderLength)
            {
                this.options = new DhcpOptionsBuffer(
                    this.buffer.Slice(HeaderLength, length - HeaderLength),
                    this.buffer.Slice(BootFileNameStart, BootFileNameLength),
                    this.buffer.Slice(ServerHostNameStart, ServerHostNameLength));
                return true;
            }

            this.options = default;
            return false;
        }

        /// <summary>
        /// Represents a sequence of DHCP options.
        /// </summary>
        public readonly struct OptionsSequence
        {
            private readonly DhcpOptionsBuffer options;

            /// <summary>
            /// Initializes a new instance of the <see cref="OptionsSequence"/> struct.
            /// </summary>
            /// <param name="options">The underlying buffer.</param>
            public OptionsSequence(DhcpOptionsBuffer options)
            {
                this.options = options;
            }

            /// <summary>
            /// Gets an enumerator which reads options in sequential order.
            /// </summary>
            /// <returns>The options enumerator.</returns>
            public DhcpOptionsBuffer.Enumerator GetEnumerator() => this.options.GetEnumerator();
        }
    }
}
