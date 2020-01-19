// <copyright file="DhcpOptionsBufferTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class DhcpOptionsBufferTest
    {
        [TestMethod]
        public void EnumerateCorruptOption()
        {
            const string ExpectedOptions =
@"AddressRequest={01020304}
End={01FFABCD000000000000}
";
            Memory<byte> options = new Memory<byte>(new byte[17]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            buffer.Slice(0, DhcpOptionTag.Pad, 1);
            DhcpOption ar = buffer.Slice(1, DhcpOptionTag.AddressRequest, 4);
            new IPAddressV4(1, 2, 3, 4).CopyTo(ar.Data.Span);
            buffer.WriteRaw(7, 0x01FFABCD);
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpOption option in buffer)
            {
                Span<char> destination = span.Slice(charsWritten);
                option.TryFormat(destination, out int c).Should().BeTrue();
                destination[c++] = '\r';
                destination[c++] = '\n';
                charsWritten += c;
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void EnumerateCorruptOptionTooShort()
        {
            const string ExpectedOptions =
@"AddressRequest={01020304}
End={0F}
";
            Memory<byte> options = new Memory<byte>(new byte[8]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            buffer.Slice(0, DhcpOptionTag.Pad, 1);
            DhcpOption ar = buffer.Slice(1, DhcpOptionTag.AddressRequest, 4);
            new IPAddressV4(1, 2, 3, 4).CopyTo(ar.Data.Span);
            buffer.WriteRaw(7, 0xF);
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpOption option in buffer)
            {
                Span<char> destination = span.Slice(charsWritten);
                option.TryFormat(destination, out int c).Should().BeTrue();
                destination[c++] = '\r';
                destination[c++] = '\n';
                charsWritten += c;
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedOptions);
        }

        [TestMethod]
        public void EnumerateCorruptSubOption()
        {
            const string ExpectedSubOptions =
@"02={06}
FF={0B0A06}
";
            Memory<byte> options = new Memory<byte>(new byte[17]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            int i = 0;
            buffer.Slice(i++, DhcpOptionTag.Pad, 1);
            buffer.WriteRaw(i++, 1); // option tag
            buffer.WriteRaw(i++, 6); // option length
            buffer.WriteRaw(i++, 2); // sub-option #1 code
            buffer.WriteRaw(i++, 1); // sub-option #1 length
            buffer.WriteRaw(i++, 6); // sub-option #1 data
            buffer.WriteRaw(i++, 11); // sub-option #2 code
            buffer.WriteRaw(i++, 10); // sub-option #2 length (BAD)
            buffer.WriteRaw(i++, 6); // sub-option #2 data
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpOption option in buffer)
            {
                foreach (DhcpSubOption subOption in option.SubOptions)
                {
                    Span<char> destination = span.Slice(charsWritten);
                    subOption.TryFormat(destination, out int c).Should().BeTrue();
                    destination[c++] = '\r';
                    destination[c++] = '\n';
                    charsWritten += c;
                }
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedSubOptions);
        }

        [TestMethod]
        public void EnumerateCorruptSubOptionTooShort()
        {
            const string ExpectedSubOptions =
@"02={06}
FF={0B}
";
            Memory<byte> options = new Memory<byte>(new byte[7]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            int i = 0;
            buffer.Slice(i++, DhcpOptionTag.Pad, 1);
            buffer.WriteRaw(i++, 1); // option tag
            buffer.WriteRaw(i++, 4); // option length
            buffer.WriteRaw(i++, 2); // sub-option #1 code
            buffer.WriteRaw(i++, 1); // sub-option #1 length
            buffer.WriteRaw(i++, 6); // sub-option #1 data
            buffer.WriteRaw(i++, 11); // sub-option #2 code
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpOption option in buffer)
            {
                foreach (DhcpSubOption subOption in option.SubOptions)
                {
                    Span<char> destination = span.Slice(charsWritten);
                    subOption.TryFormat(destination, out int c).Should().BeTrue();
                    destination[c++] = '\r';
                    destination[c++] = '\n';
                    charsWritten += c;
                }
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedSubOptions);
        }

        [TestMethod]
        public void EnumerateCorruptRelayAgentInformation()
        {
            const string ExpectedSubOptions =
@"AgentCircuitId={61}
None={FE0A06}
";
            Memory<byte> options = new Memory<byte>(new byte[17]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            DhcpOption option = buffer.Slice(0, DhcpOptionTag.RelayAgentInformation, 6);
            int i = 0;
            option.Data.Span[i++] = (byte)DhcpRelayAgentSubOptionCode.AgentCircuitId;
            option.Data.Span[i++] = 1;         // sub-option #1 length
            option.Data.Span[i++] = (byte)'a'; // sub-option #1 data
            option.Data.Span[i++] = 254;       // sub-option #2 code
            option.Data.Span[i++] = 10;        // sub-option #2 length (BAD)
            option.Data.Span[i++] = 6;         // sub-option #2 data
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpRelayAgentInformationSubOption subOption in option.RelayAgentInformation())
            {
                Span<char> destination = span.Slice(charsWritten);
                subOption.TryFormat(destination, out int c).Should().BeTrue();
                destination[c++] = '\r';
                destination[c++] = '\n';
                charsWritten += c;
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedSubOptions);
        }

        [TestMethod]
        public void EnumerateCorruptRelayAgentInformationTooShort()
        {
            const string ExpectedSubOptions =
@"AgentCircuitId={61}
None={FE}
";
            Memory<byte> options = new Memory<byte>(new byte[6]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            DhcpOption option = buffer.Slice(0, DhcpOptionTag.RelayAgentInformation, 4);
            int i = 0;
            option.Data.Span[i++] = (byte)DhcpRelayAgentSubOptionCode.AgentCircuitId;
            option.Data.Span[i++] = 1;         // sub-option #1 length
            option.Data.Span[i++] = (byte)'a'; // sub-option #1 data
            option.Data.Span[i++] = 254;       // sub-option #2 code
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpRelayAgentInformationSubOption subOption in option.RelayAgentInformation())
            {
                Span<char> destination = span.Slice(charsWritten);
                subOption.TryFormat(destination, out int c).Should().BeTrue();
                destination[c++] = '\r';
                destination[c++] = '\n';
                charsWritten += c;
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedSubOptions);
        }

        [TestMethod]
        public void EnumerateCorruptRadiusAttribute()
        {
            const string ExpectedRadiusAttributes =
@"UserName={61}
None={FE080A00000000}
";
            Memory<byte> options = new Memory<byte>(new byte[17]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            DhcpOption option = buffer.Slice(0, DhcpOptionTag.RelayAgentInformation, 12);
            int i = 0;
            option.Data.Span[i++] = (byte)DhcpRelayAgentSubOptionCode.RadiusAttributes;
            option.Data.Span[i++] = 10;        // sub-option #1 length
            option.Data.Span[i++] = (byte)RadiusAttributeType.UserName;
            option.Data.Span[i++] = 3;         // attribute #1 length
            option.Data.Span[i++] = (byte)'a'; // attribute #1 data
            option.Data.Span[i++] = 254;       // attribute #2 code
            option.Data.Span[i++] = 8;         // attribute #2 length (BAD)
            option.Data.Span[i++] = 10;        // attribute #2 data
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpRelayAgentInformationSubOption subOption in option.RelayAgentInformation())
            {
                foreach (RadiusAttribute attribute in subOption.RadiusAttributes())
                {
                    Span<char> destination = span.Slice(charsWritten);
                    attribute.TryFormat(destination, out int c).Should().BeTrue();
                    destination[c++] = '\r';
                    destination[c++] = '\n';
                    charsWritten += c;
                }
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedRadiusAttributes);
        }

        [TestMethod]
        public void EnumerateCorruptRadiusAttributeUnderflow()
        {
            const string ExpectedRadiusAttributes =
@"UserName={61}
None={FE010A00000000}
";
            Memory<byte> options = new Memory<byte>(new byte[17]);
            DhcpOptionsBuffer buffer = new DhcpOptionsBuffer(options, Memory<byte>.Empty, Memory<byte>.Empty);
            DhcpOption option = buffer.Slice(0, DhcpOptionTag.RelayAgentInformation, 12);
            int i = 0;
            option.Data.Span[i++] = (byte)DhcpRelayAgentSubOptionCode.RadiusAttributes;
            option.Data.Span[i++] = 10;        // sub-option #1 length
            option.Data.Span[i++] = (byte)RadiusAttributeType.UserName;
            option.Data.Span[i++] = 3;         // attribute #1 length
            option.Data.Span[i++] = (byte)'a'; // attribute #1 data
            option.Data.Span[i++] = 254;       // attribute #2 code
            option.Data.Span[i++] = 1;         // attribute #2 length (BAD -> will underflow)
            option.Data.Span[i++] = 10;        // attribute #2 data
            Span<char> span = new Span<char>(new char[100]);

            int charsWritten = 0;
            foreach (DhcpRelayAgentInformationSubOption subOption in option.RelayAgentInformation())
            {
                foreach (RadiusAttribute attribute in subOption.RadiusAttributes())
                {
                    Span<char> destination = span.Slice(charsWritten);
                    attribute.TryFormat(destination, out int c).Should().BeTrue();
                    destination[c++] = '\r';
                    destination[c++] = '\n';
                    charsWritten += c;
                }
            }

            span.Slice(0, charsWritten).ToString().Should().Be(ExpectedRadiusAttributes);
        }
    }
}