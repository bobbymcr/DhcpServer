// <copyright file="IPAddressV4Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Net;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class IPAddressV4Test
    {
        [TestMethod]
        public void Equality()
        {
            IPAddressV4 zero = default;
            IPAddressV4 one1 = new IPAddressV4(1, 2, 3, 4);
            IPAddressV4 one2 = new IPAddressV4(0x01020304);
            IPAddressV4 two = new IPAddressV4(0xFF, 0xFF, 0xFF, 0xFF);

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
        public void WriteToExact()
        {
            IPAddressV4 address = new IPAddressV4(1, 2, 3, 4);
            byte[] raw = new byte[4];
            Span<byte> span = new Span<byte>(raw);

            address.WriteTo(span);

            raw.Should().ContainInOrder(1, 2, 3, 4);
        }

        [TestMethod]
        public void Conversion()
        {
            IPAddressV4 max = new IPAddressV4(uint.MaxValue);
            IPAddressV4 middle = new IPAddressV4(0x40, 0x3F, 0x41, 0x7F);
            IPAddressV4 min = new IPAddressV4(0);

            TestConvert(max, 0xFFFFFFFF, "255.255.255.255");
            TestConvert(middle, 0x7F413F40, "64.63.65.127");
            TestConvert(min, 0, "0.0.0.0");
        }

        [TestMethod]
        public void TryFormat()
        {
            TestTryFormat(0x00010203, "0.1.2.3");
            TestTryFormat(0x04050607, "4.5.6.7");
            TestTryFormat(0x08090A0B, "8.9.10.11");
            TestTryFormat(0x0C0D0E0F, "12.13.14.15");
            TestTryFormat(0x10111213, "16.17.18.19");
            TestTryFormat(0x14151617, "20.21.22.23");
            TestTryFormat(0x18191A1B, "24.25.26.27");
            TestTryFormat(0x1C1D1E1F, "28.29.30.31");
            TestTryFormat(0x20212223, "32.33.34.35");
            TestTryFormat(0x24252627, "36.37.38.39");
            TestTryFormat(0x28292A2B, "40.41.42.43");
            TestTryFormat(0x2C2D2E2F, "44.45.46.47");
            TestTryFormat(0x30313233, "48.49.50.51");
            TestTryFormat(0x34353637, "52.53.54.55");
            TestTryFormat(0x38393A3B, "56.57.58.59");
            TestTryFormat(0x3C3D3E3F, "60.61.62.63");
            TestTryFormat(0x40414243, "64.65.66.67");
            TestTryFormat(0x44454647, "68.69.70.71");
            TestTryFormat(0x48494A4B, "72.73.74.75");
            TestTryFormat(0x4C4D4E4F, "76.77.78.79");
            TestTryFormat(0x50515253, "80.81.82.83");
            TestTryFormat(0x54555657, "84.85.86.87");
            TestTryFormat(0x58595A5B, "88.89.90.91");
            TestTryFormat(0x5C5D5E5F, "92.93.94.95");
            TestTryFormat(0x60616263, "96.97.98.99");
            TestTryFormat(0x64656667, "100.101.102.103");
            TestTryFormat(0x68696A6B, "104.105.106.107");
            TestTryFormat(0x6C6D6E6F, "108.109.110.111");
            TestTryFormat(0x70717273, "112.113.114.115");
            TestTryFormat(0x74757677, "116.117.118.119");
            TestTryFormat(0x78797A7B, "120.121.122.123");
            TestTryFormat(0x7C7D7E7F, "124.125.126.127");
            TestTryFormat(0x80818283, "128.129.130.131");
            TestTryFormat(0x84858687, "132.133.134.135");
            TestTryFormat(0x88898A8B, "136.137.138.139");
            TestTryFormat(0x8C8D8E8F, "140.141.142.143");
            TestTryFormat(0x90919293, "144.145.146.147");
            TestTryFormat(0x94959697, "148.149.150.151");
            TestTryFormat(0x98999A9B, "152.153.154.155");
            TestTryFormat(0x9C9D9E9F, "156.157.158.159");
            TestTryFormat(0xA0A1A2A3, "160.161.162.163");
            TestTryFormat(0xA4A5A6A7, "164.165.166.167");
            TestTryFormat(0xA8A9AAAB, "168.169.170.171");
            TestTryFormat(0xACADAEAF, "172.173.174.175");
            TestTryFormat(0xB0B1B2B3, "176.177.178.179");
            TestTryFormat(0xB4B5B6B7, "180.181.182.183");
            TestTryFormat(0xB8B9BABB, "184.185.186.187");
            TestTryFormat(0xBCBDBEBF, "188.189.190.191");
            TestTryFormat(0xC0C1C2C3, "192.193.194.195");
            TestTryFormat(0xC4C5C6C7, "196.197.198.199");
            TestTryFormat(0xC8C9CACB, "200.201.202.203");
            TestTryFormat(0xCCCDCECF, "204.205.206.207");
            TestTryFormat(0xD0D1D2D3, "208.209.210.211");
            TestTryFormat(0xD4D5D6D7, "212.213.214.215");
            TestTryFormat(0xD8D9DADB, "216.217.218.219");
            TestTryFormat(0xDCDDDEDF, "220.221.222.223");
            TestTryFormat(0xE0E1E2E3, "224.225.226.227");
            TestTryFormat(0xE4E5E6E7, "228.229.230.231");
            TestTryFormat(0xE8E9EAEB, "232.233.234.235");
            TestTryFormat(0xECEDEEEF, "236.237.238.239");
            TestTryFormat(0xF0F1F2F3, "240.241.242.243");
            TestTryFormat(0xF4F5F6F7, "244.245.246.247");
            TestTryFormat(0xF8F9FAFB, "248.249.250.251");
            TestTryFormat(0xFCFDFEFF, "252.253.254.255");
        }

        [TestMethod]
        public void TryFormatTooSmall()
        {
            TestTryFormatTooSmall(0x01020304, 1);
            TestTryFormatTooSmall(0x01020304, 2);
            TestTryFormatTooSmall(0x01020304, 3);
            TestTryFormatTooSmall(0x01020304, 4);
            TestTryFormatTooSmall(0x01020304, 5);
            TestTryFormatTooSmall(0x01020304, 6);
            TestTryFormatTooSmall(0x0F0E0D0C, 7);
            TestTryFormatTooSmall(0x0F0E0D0C, 8);
            TestTryFormatTooSmall(0x0F0E0D0C, 9);
            TestTryFormatTooSmall(0x0F0E0D0C, 10);
            TestTryFormatTooSmall(0x2030407F, 11);
            TestTryFormatTooSmall(0x2030EF7F, 12);
            TestTryFormatTooSmall(0x20DCEF7F, 13);
            TestTryFormatTooSmall(0xFFFFFFFF, 14);
        }

        private static void TestTryFormatTooSmall(uint input, int badLength)
        {
            char[] array = new char[badLength];
            IPAddressV4 address = new IPAddressV4(input);

            bool result = address.TryFormat(new Span<char>(array), out int charsWritten);

            result.Should().BeFalse(because: "{0:X8} needs more than {1} chars", input, badLength);
            charsWritten.Should().Be(0);
            array.Should().OnlyContain(c => c == '\0');
        }

        private static void TestTryFormat(uint input, string expected)
        {
            Span<char> destination = new Span<char>(new char[32]);
            IPAddressV4 address = new IPAddressV4(input);

            bool result = address.TryFormat(destination, out int charsWritten);

            result.Should().BeTrue();
            charsWritten.Should().Be(expected.Length);
            destination.Slice(0, charsWritten).ToString().Should().Be(expected);
        }

        private static void TestConvert(IPAddressV4 value, uint expected, string expectedStr)
        {
            uint i = (uint)value;
            IPAddress address = new IPAddress(i);

            i.Should().Be(expected);
            address.ToString().Should().Be(expectedStr);
        }
    }
}
