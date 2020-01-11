// <copyright file="MacAddressTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class MacAddressTest
    {
        [TestMethod]
        public void Equality()
        {
            MacAddress zero = default;
            MacAddress one1 = new MacAddress(1, 2, 3, 4, 5, 6);
            MacAddress one2 = new MacAddress(0x010203040506);
            MacAddress two = new MacAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);

            EqualityTest.Check(true, one1, one2);
            EqualityTest.Check(false, one1, two);
            EqualityTest.Check(false, one1, zero);
            EqualityTest.Check(true, zero, zero);
            one1.Equals("hmm").Should().BeFalse();
            zero.Equals(null).Should().BeFalse();

            one1.GetHashCode().Should().Be(one2.GetHashCode());
            one1.GetHashCode().Should().NotBe(two.GetHashCode());
            zero.GetHashCode().Should().NotBe(one1.GetHashCode());
        }

        [TestMethod]
        public void WriteToWithPadding()
        {
            MacAddress address = new MacAddress(0xFFFF123456789ABC);
            byte[] raw = new byte[9];
            Span<byte> span = new Span<byte>(raw);

            address.WriteTo(span);

            raw.Should().ContainInOrder(0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0, 0, 0);
        }

        [TestMethod]
        public void WriteToExact()
        {
            MacAddress address = new MacAddress(0xA, 0xB, 0xC, 0xD, 0xE, 0xF);
            byte[] raw = new byte[6];
            Span<byte> span = new Span<byte>(raw);

            address.WriteTo(span);

            raw.Should().ContainInOrder(0xA, 0xB, 0xC, 0xD, 0xE, 0xF);
        }

        [TestMethod]
        public void TryFormat()
        {
            TestTryFormat(0xFEDCBA987654, "FE-DC-BA-98-76-54");
            TestTryFormat(0x000000000000, "00-00-00-00-00-00");
            TestTryFormat(0x00123456789F, "00-12-34-56-78-9F");
        }

        [TestMethod]
        public void TryFormatD()
        {
            TestTryFormat("D", 0xFEDCBA987654, "FE-DC-BA-98-76-54");
            TestTryFormat("D", 0x000000000000, "00-00-00-00-00-00");
            TestTryFormat("D", 0x00123456789F, "00-12-34-56-78-9F");
        }

        [TestMethod]
        public void TryFormatN()
        {
            TestTryFormat("N", 0xFEDCBA987654, "FEDCBA987654");
            TestTryFormat("N", 0x000000000000, "000000000000");
            TestTryFormat("N", 0x00123456789F, "00123456789F");
        }

        [TestMethod]
        public void TryFormatEmpty()
        {
            TestTryFormat(string.Empty, 0xFEDCBA987654, "FE-DC-BA-98-76-54");
            TestTryFormat(string.Empty, 0x000000000000, "00-00-00-00-00-00");
            TestTryFormat(string.Empty, 0x00123456789F, "00-12-34-56-78-9F");
        }

        [TestMethod]
        public void TryFormatNull()
        {
            TestTryFormat(null, 0xFEDCBA987654, "FE-DC-BA-98-76-54");
            TestTryFormat(null, 0x000000000000, "00-00-00-00-00-00");
            TestTryFormat(null, 0x00123456789F, "00-12-34-56-78-9F");
        }

        [TestMethod]
        public void TryFormatBufferTooSmall()
        {
            Span<char> destination = new Span<char>(new char[1]);
            MacAddress address = new MacAddress(1);

            bool result = address.TryFormat(destination, out int charsWritten);

            result.Should().BeFalse();
            charsWritten.Should().Be(0);
            destination[0].Should().Be('\0');
        }

        private static void TestTryFormat(ulong input, string expected)
        {
            Memory<char> destination = new Memory<char>(new char[32]);
            MacAddress address = new MacAddress(input);

            bool result = address.TryFormat(destination.Span, out int charsWritten);

            result.Should().BeTrue();
            charsWritten.Should().Be(expected.Length);
            destination.Slice(0, charsWritten).ToString().Should().Be(expected);
        }

        private static void TestTryFormat(string format, ulong input, string expected)
        {
            Memory<char> destination = new Memory<char>(new char[32]);
            MacAddress address = new MacAddress(input);

            bool result = address.TryFormat(destination.Span, out int charsWritten, format);

            result.Should().BeTrue();
            charsWritten.Should().Be(expected.Length);
            destination.Slice(0, charsWritten).ToString().Should().Be(expected);
        }
    }
}
