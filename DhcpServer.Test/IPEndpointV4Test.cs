// <copyright file="IPEndpointV4Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class IPEndpointV4Test
    {
        [TestMethod]
        public void Equality()
        {
            IPEndpointV4 zero = default;
            IPEndpointV4 one1 = new IPEndpointV4(new IPAddressV4(1, 2, 3, 4), 0xFFFF);
            IPEndpointV4 one2 = new IPEndpointV4(new IPAddressV4(0x01020304), 65535);
            IPEndpointV4 two = new IPEndpointV4(new IPAddressV4(1, 2, 3, 4), 5);
            IPEndpointV4 three = new IPEndpointV4(new IPAddressV4(5, 6, 7, 8), 0xFFFF);
            IPEndpointV4 four = new IPEndpointV4(new IPAddressV4(5, 6, 7, 8), 5);

            EqualityTest.Check(true, one1, one2);
            EqualityTest.Check(false, one1, two);
            EqualityTest.Check(false, one1, zero);
            EqualityTest.Check(true, zero, zero);
            EqualityTest.Check(false, one1, three);
            EqualityTest.Check(false, one2, four);
            one1.Equals("hmm").Should().BeFalse();
            zero.Equals(null).Should().BeFalse();

            one1.GetHashCode().Should().Be(one2.GetHashCode());
            one1.GetHashCode().Should().NotBe(two.GetHashCode());
            one1.GetHashCode().Should().NotBe(three.GetHashCode());
            zero.GetHashCode().Should().NotBe(one1.GetHashCode());
            three.GetHashCode().Should().NotBe(four.GetHashCode());
        }

        [TestMethod]
        public void TryFormat()
        {
            TestTryFormat(0x01020304, 1, "1.2.3.4:1");
            TestTryFormat(0x10020304, 23, "16.2.3.4:23");
            TestTryFormat(0x10200304, 456, "16.32.3.4:456");
            TestTryFormat(0x10203004, 7890, "16.32.48.4:7890");
            TestTryFormat(0x10203040, 12345, "16.32.48.64:12345");
            TestTryFormat(0x10203064, 0, "16.32.48.100:0");
            TestTryFormat(0x1020C864, 78, "16.32.200.100:78");
            TestTryFormat(0x107FC864, 9007, "16.127.200.100:9007");
            TestTryFormat(0xFFFFFFFF, 65535, "255.255.255.255:65535");
        }

        [TestMethod]
        public void TryFormatTooSmall()
        {
            TestTryFormatTooSmall(0x01020304, 1, 1);
            TestTryFormatTooSmall(0x01020304, 2, 2);
            TestTryFormatTooSmall(0x01020304, 3, 3);
            TestTryFormatTooSmall(0x01020304, 4, 4);
            TestTryFormatTooSmall(0x01020304, 5, 5);
            TestTryFormatTooSmall(0x01020304, 6, 6);
            TestTryFormatTooSmall(0x01020304, 7, 7);
            TestTryFormatTooSmall(0x01020304, 8, 8);
            TestTryFormatTooSmall(0x01020304, 8, 8);
            TestTryFormatTooSmall(0x01020304, 10, 9);
            TestTryFormatTooSmall(0x01020304, 200, 10);
            TestTryFormatTooSmall(0x01020304, 2000, 11);
            TestTryFormatTooSmall(0x01020304, 20000, 12);
            TestTryFormatTooSmall(0x0102030A, 30000, 13);
            TestTryFormatTooSmall(0x01020B0A, 40000, 14);
            TestTryFormatTooSmall(0x010C0B0A, 50000, 15);
            TestTryFormatTooSmall(0x0D0C0B0A, 60000, 16);
            TestTryFormatTooSmall(0x0D0CB0A0, 6000, 17);
            TestTryFormatTooSmall(0xD0C0B0A0, 600, 18);
            TestTryFormatTooSmall(0xD0C0B0A0, 6000, 19);
            TestTryFormatTooSmall(0xD0C0B0A0, 60000, 20);
        }

        private static void TestTryFormatTooSmall(uint address, ushort port, int badLength)
        {
            char[] array = new char[badLength];
            IPEndpointV4 endpoint = new IPEndpointV4(new IPAddressV4(address), port);

            bool result = endpoint.TryFormat(new Span<char>(array), out int charsWritten);

            result.Should().BeFalse(because: "{0:X8}:{1} needs more than {2} chars", address, port, badLength);
            charsWritten.Should().Be(0);
            array.Should().OnlyContain(c => c == '\0');
        }

        private static void TestTryFormat(uint address, ushort port, string expected)
        {
            Span<char> destination = new Span<char>(new char[32]);
            IPEndpointV4 endpoint = new IPEndpointV4(new IPAddressV4(address), port);

            bool result = endpoint.TryFormat(destination, out int charsWritten);

            result.Should().BeTrue();
            charsWritten.Should().Be(expected.Length);
            destination.Slice(0, charsWritten).ToString().Should().Be(expected);
        }
    }
}
