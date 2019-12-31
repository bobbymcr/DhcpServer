// <copyright file="IPEndpointV4Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
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
    }
}
