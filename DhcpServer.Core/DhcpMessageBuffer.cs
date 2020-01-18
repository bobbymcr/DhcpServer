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
        private const byte ClientHardwareAddressStart = 28;
        private const byte MaxHardwareAddressLength = 16;
        private const byte HeaderLength = 240;
        private const byte ServerHostNameStart = 44;
        private const byte ServerHostNameLength = 64;
        private const byte BootFileNameStart = 108;
        private const byte BootFileNameLength = 128;

        private readonly Memory<byte> buffer;

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
            this.buffer = buffer;
            if (!this.SetOptions(buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(buffer));
            }
        }

        /// <summary>
        /// Gets the length of the currently buffered message.
        /// </summary>
        /// <remarks>This value is set after a successful <see cref="Load(int)"/> or <see cref="Save"/> operation.</remarks>
        public int Length { get; private set; }

        /// <summary>
        /// Gets a span for the underlying buffer.
        /// </summary>
        /// <remarks>The span length covers the entire buffer, not just the section with valid message data.</remarks>
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
        /// <remarks>The <see cref="HardwareAddressLength"/> value must be set before calling this.
        /// Regardless of the <see cref="HardwareAddressLength"/> value, the span will never be longer than 16.</remarks>
        public Span<byte> ClientHardwareAddress
        {
            get
            {
                return this.Span.Slice(
                    ClientHardwareAddressStart,
                    Math.Min(this.HardwareAddressLength, MaxHardwareAddressLength));
            }
        }

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

            this.Length = length;
            this.Opcode = (DhcpOpcode)this.buffer.ParseUInt8(0);
            this.HardwareAddressType = (DhcpHardwareAddressType)this.buffer.ParseUInt8(1);
            this.HardwareAddressLength = this.buffer.ParseUInt8(2);
            this.Hops = this.buffer.ParseUInt8(3);
            this.TransactionId = this.buffer.ParseUInt32(4);
            this.Seconds = this.buffer.ParseUInt16(8);
            this.Flags = (DhcpFlags)this.buffer.ParseUInt16(10);
            this.ClientIPAddress = this.buffer.ParseIPAddressV4(12);
            this.YourIPAddress = this.buffer.ParseIPAddressV4(16);
            this.ServerIPAddress = this.buffer.ParseIPAddressV4(20);
            this.GatewayIPAddress = this.buffer.ParseIPAddressV4(24);
            this.MagicCookie = (MagicCookie)this.buffer.ParseUInt32(236);
            return true;
        }

        /// <summary>
        /// Saves message data to the underlying buffer and resets the cursor.
        /// </summary>
        /// <returns>The total message length.</returns>
        public int Save()
        {
            ((byte)this.Opcode).CopyTo(this.buffer, 0);
            ((byte)this.HardwareAddressType).CopyTo(this.buffer, 1);
            this.HardwareAddressLength.CopyTo(this.buffer, 2);
            this.Hops.CopyTo(this.buffer, 3);
            this.TransactionId.CopyTo(this.buffer, 4);
            this.Seconds.CopyTo(this.buffer, 8);
            ((ushort)this.Flags).CopyTo(this.buffer, 10);
            this.ClientIPAddress.CopyTo(this.buffer, 12);
            this.YourIPAddress.CopyTo(this.buffer, 16);
            this.ServerIPAddress.CopyTo(this.buffer, 20);
            this.GatewayIPAddress.CopyTo(this.buffer, 24);
            ((uint)this.MagicCookie).CopyTo(this.buffer, 236);
            int totalLength = this.nextOption + HeaderLength;
            this.nextOption = 0;
            return this.Length = totalLength;
        }

        /// <summary>
        /// Writes a container option header to the buffer with a variable sized data segment and advances the cursor.
        /// </summary>
        /// <remarks>At least one call to
        /// <see cref="WriteSubOptionHeader(byte, byte)"/>,
        /// <see cref="WriteSubOption(byte, ReadOnlySpan{char}, Encoding)"/>,
        /// <see cref="WriteOptionRaw(byte)"/>,
        /// <see cref="WriteOptionRaw(uint)"/>, or
        /// <see cref="WriteOptionRaw(ReadOnlySpan{char}, Encoding)"/>
        /// must be made after this.
        /// The container option must be terminated by an <see cref="EndContainerOption()"/> or
        /// <see cref="EndContainerOption(ReadOnlySpan{byte})"/> call.</remarks>
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
        /// Marks the end of the container option by writing an option end code followed by
        /// a raw data segment. The length is updated to include the total sub-options
        /// and raw data size.
        /// </summary>
        /// <remarks>At least one call to either <see cref="WriteSubOptionHeader(byte, byte)"/> or
        /// <see cref="WriteSubOption(byte, ReadOnlySpan{char}, Encoding)"/> must be made before this.
        /// </remarks>
        /// <param name="data">The data buffer.</param>
        public void EndContainerOption(ReadOnlySpan<byte> data)
        {
            this.WriteEndOption();
            data.CopyTo(this.Span.Slice(HeaderLength + this.nextOption));
            this.nextOption += data.Length;
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
        /// Slices out a data segment from the cursor to the end of the options buffer.
        /// </summary>
        /// <returns>The sliced option data segment.</returns>
        public Memory<byte> SliceOptionRaw() => this.options.Slice(this.nextOption);

        /// <summary>
        /// Writes raw option data to the buffer and advances the cursor.
        /// </summary>
        /// <param name="value">The data value.</param>
        public void WriteOptionRaw(byte value) => this.options.WriteRaw(this.nextOption++, value);

        /// <summary>
        /// Writes raw option data to the buffer and advances the cursor.
        /// </summary>
        /// <param name="value">The data value.</param>
        public void WriteOptionRaw(uint value)
        {
            this.options.WriteRaw(this.nextOption, value);
            this.nextOption += 4;
        }

        /// <summary>
        /// Writes raw option data to the buffer and advances the cursor.
        /// </summary>
        /// <param name="chars">The character data.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <returns>The number of bytes written.</returns>
        public byte WriteOptionRaw(ReadOnlySpan<char> chars, Encoding encoding)
        {
            byte length = this.options.WriteRaw(this.nextOption, chars, encoding);
            this.nextOption += length;
            return length;
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

        /// <summary>
        /// Tries to format the fields of the current message into the provided span of characters.
        /// </summary>
        /// <param name="destination">When this method returns, this message's fields formatted as a span of characters.</param>
        /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
        /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            charsWritten = 0;
            return
                NextField(destination, ref charsWritten, "op", this.Opcode.ToString()) &&
                NextField(destination, ref charsWritten, "htype", this.HardwareAddressType.ToString()) &&
                NextField(destination, ref charsWritten, "hlen", this.HardwareAddressLength) &&
                NextField(destination, ref charsWritten, "hops", this.Hops) &&
                NextField(destination, ref charsWritten, "xid", this.TransactionId) &&
                NextField(destination, ref charsWritten, "secs", this.Seconds) &&
                NextField(destination, ref charsWritten, "flags", this.Flags.ToString()) &&
                NextField(destination, ref charsWritten, "ciaddr", this.ClientIPAddress) &&
                NextField(destination, ref charsWritten, "yiaddr", this.YourIPAddress) &&
                NextField(destination, ref charsWritten, "siaddr", this.ServerIPAddress) &&
                NextField(destination, ref charsWritten, "giaddr", this.GatewayIPAddress) &&
                NextField(destination, ref charsWritten, "chaddr", this.ClientHardwareAddress) &&
                NextFieldAscii(destination, ref charsWritten, "sname", this.ServerHostName) &&
                NextFieldAscii(destination, ref charsWritten, "file", this.BootFileName) &&
                LastField(destination, ref charsWritten, "magic", this.MagicCookie.ToString());
        }

        private static bool LastField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, ReadOnlySpan<char> value)
        {
            bool result = Field.TryFormat(destination.Slice(charsWritten), out int c, field, value);
            charsWritten += c;
            return result;
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, ReadOnlySpan<char> value)
        {
            bool result = false;
            if (Field.TryFormat(destination.Slice(charsWritten), out int c, field, value))
            {
                charsWritten += c;
                result = Field.TryAppend(destination.Slice(charsWritten), out c, "; ");
                charsWritten += c;
            }

            return result;
        }

        private static bool NextFieldAscii(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, Span<byte> raw)
        {
            int n = raw.Length;
            Span<char> value = stackalloc char[n + 2];
            value[0] = '\'';
            int i = 0;
            while (i < n)
            {
                char c = (char)raw[i];
                if (c == '\0')
                {
                    break;
                }

                value[++i] = c;
            }

            value[++i] = '\'';

            return NextField(destination, ref charsWritten, field, value.Slice(0, i + 1));
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, Span<byte> raw)
        {
            Span<char> value = stackalloc char[raw.Length * 2];
            for (int i = 0; i < raw.Length; ++i)
            {
                Hex.Format(value, 2 * i, raw[i]);
            }

            return NextField(destination, ref charsWritten, field, value);
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, IPAddressV4 ip)
        {
            Span<char> value = stackalloc char[15];
            ip.TryFormat(value, out int len);
            return NextField(destination, ref charsWritten, field, value.Slice(0, len));
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, ushort num)
        {
            Span<char> value = stackalloc char[5];
            if (num > 9999)
            {
                Base10.FormatDigits5(value, 0, num);
            }
            else if (num > 999)
            {
                Base10.FormatDigits4(value, 0, num);
                value = value.Slice(0, 4);
            }
            else if (num > 99)
            {
                Base10.FormatDigits3(value, 0, num);
                value = value.Slice(0, 3);
            }
            else if (num > 9)
            {
                Base10.FormatDigits2(value, 0, (byte)num);
                value = value.Slice(0, 2);
            }
            else
            {
                Base10.FormatDigit(value, 0, (byte)num);
                value = value.Slice(0, 1);
            }

            return NextField(destination, ref charsWritten, field, value);
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, uint num)
        {
            Span<char> value = stackalloc char[10];
            value[0] = '0';
            value[1] = 'x';
            Hex.Format(value, 2, (byte)(num >> 24));
            Hex.Format(value, 4, (byte)(num >> 16));
            Hex.Format(value, 6, (byte)(num >> 8));
            Hex.Format(value, 8, (byte)(num & 0xFF));

            return NextField(destination, ref charsWritten, field, value);
        }

        private static bool NextField(Span<char> destination, ref int charsWritten, ReadOnlySpan<char> field, byte num)
        {
            Span<char> value = stackalloc char[3];
            if (num > 99)
            {
                Base10.FormatDigits3(value, 0, num);
            }
            else if (num > 9)
            {
                Base10.FormatDigits2(value, 0, num);
                value = value.Slice(0, 2);
            }
            else
            {
                Base10.FormatDigit(value, 0, num);
                value = value.Slice(0, 1);
            }

            return NextField(destination, ref charsWritten, field, value);
        }

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

            /// <summary>
            /// Tries to format the current option sequence into the provided span of characters.
            /// </summary>
            /// <param name="destination">When this method returns, this options formatted as a span of characters.</param>
            /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination"/>.</param>
            /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
            public bool TryFormat(Span<char> destination, out int charsWritten)
            {
                // Final result should be options separated by newlines, e.g. '<option1>NL<option2>NL...'
                charsWritten = 0;
                foreach (DhcpOption option in this)
                {
                    bool result = option.TryFormat(destination.Slice(charsWritten), out int c);
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
