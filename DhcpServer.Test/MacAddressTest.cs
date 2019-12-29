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

            CheckEquality(true, one1, one2);
            CheckEquality(false, one1, two);
            CheckEquality(false, one1, zero);
            CheckEquality(true, zero, zero);
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

        private static void CheckEquality<T>(bool areEqual, T x, T y)
            where T : IEquatable<T>
        {
            x.Equals(y).Should().Be(areEqual);
            y.Equals(x).Should().Be(areEqual);
            ((object)x).Equals(y).Should().Be(areEqual);
            y.Equals((object)x).Should().Be(areEqual);
        }
    }
}
