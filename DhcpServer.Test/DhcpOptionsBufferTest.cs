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
    }
}