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
