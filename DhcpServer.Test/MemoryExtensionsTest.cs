// <copyright file="MemoryExtensionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class MemoryExtensionsTest
    {
        [TestMethod]
        public void ParseCopyUInt8()
        {
            byte[] raw = new byte[] { 10, 20 };
            Memory<byte> buffer = new Memory<byte>(raw);

            buffer.ParseUInt8(0).Should().Be(10);
            buffer.ParseUInt8(1).Should().Be(20);

            ((byte)30).CopyTo(buffer, 0);
            ((byte)40).CopyTo(buffer, 1);

            raw.Should().ContainInOrder(30, 40);
            buffer.ParseUInt8(0).Should().Be(30);
            buffer.ParseUInt8(1).Should().Be(40);
        }

        [TestMethod]
        public void ParseCopyUInt16()
        {
            byte[] raw = new byte[] { 0xEF, 0xCD, 0x00, 0x99 };
            Memory<byte> buffer = new Memory<byte>(raw);

            buffer.ParseUInt16(0).Should().Be(0xEFCD);
            buffer.ParseUInt16(2).Should().Be(0x0099);

            ((ushort)0).CopyTo(buffer, 0);
            ((ushort)0xABCD).CopyTo(buffer, 2);

            raw.Should().ContainInOrder(0, 0, 0xAB, 0xCD);
            buffer.ParseUInt16(0).Should().Be(0);
            buffer.ParseUInt16(2).Should().Be(0xABCD);
        }

        [TestMethod]
        public void ParseCopyUInt32()
        {
            byte[] raw = new byte[] { 0xFC, 0x22, 0x33, 0x44, 0x00, 0xFF, 0x78, 0x00 };
            Memory<byte> buffer = new Memory<byte>(raw);

            buffer.ParseUInt32(0).Should().Be(0xFC223344);
            buffer.ParseUInt32(4).Should().Be(0xFF7800U);

            0U.CopyTo(buffer, 0);
            0xABCDEF11.CopyTo(buffer, 4);

            raw.Should().ContainInOrder(0, 0, 0, 0, 0xAB, 0xCD, 0xEF, 0x11);
            buffer.ParseUInt32(0).Should().Be(0U);
            buffer.ParseUInt32(4).Should().Be(0xABCDEF11);
        }

        [TestMethod]
        public void ParseCopyIP()
        {
            byte[] raw = new byte[] { 0x01, 0x02, 0x03, 0x05, 0x00, 0x08, 0xA, 0x00 };
            Memory<byte> buffer = new Memory<byte>(raw);

            buffer.ParseIPAddressV4(0).Should().Be(new IPAddressV4(1, 2, 3, 5));
            buffer.ParseIPAddressV4(4).Should().Be(new IPAddressV4(0, 8, 10, 0));

            new IPAddressV4(7, 8, 9, 0).CopyTo(buffer, 0);
            new IPAddressV4(0xC0, 0x7F, 0xFF, 0xFF).CopyTo(buffer, 4);

            raw.Should().ContainInOrder(7, 8, 9, 0, 0xC0, 0x7F, 0xFF, 0xFF);
            buffer.ParseIPAddressV4(0).Should().Be(new IPAddressV4(7, 8, 9, 0));
            buffer.ParseIPAddressV4(4).Should().Be(new IPAddressV4(0xC0, 0x7F, 0xFF, 0xFF));
        }
    }
}
