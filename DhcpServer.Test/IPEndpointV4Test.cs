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
