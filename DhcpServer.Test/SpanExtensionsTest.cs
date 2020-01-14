// <copyright file="SpanExtensionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class SpanExtensionsTest
    {
        [TestMethod]
        public void ParseCopyUInt8()
        {
            byte[] raw = new byte[] { 10, 20 };
            Span<byte> buffer = new Memory<byte>(raw).Span;

            buffer.Slice(0).ParseUInt8().Should().Be(10);
            buffer.Slice(1).ParseUInt8().Should().Be(20);

            ((byte)30).CopyTo(buffer.Slice(0));
            ((byte)40).CopyTo(buffer.Slice(1));

            raw.Should().ContainInOrder(30, 40);
            buffer.Slice(0).ParseUInt8().Should().Be(30);
            buffer.Slice(1).ParseUInt8().Should().Be(40);
        }

        [TestMethod]
        public void ParseCopyUInt16()
        {
            byte[] raw = new byte[] { 0xEF, 0xCD, 0x00, 0x99 };
            Span<byte> buffer = new Memory<byte>(raw).Span;

            buffer.Slice(0).ParseUInt16().Should().Be(0xEFCD);
            buffer.Slice(2).ParseUInt16().Should().Be(0x0099);

            ((ushort)0).CopyTo(buffer.Slice(0));
            ((ushort)0xABCD).CopyTo(buffer.Slice(2));

            raw.Should().ContainInOrder(0, 0, 0xAB, 0xCD);
            buffer.Slice(0).ParseUInt16().Should().Be(0);
            buffer.Slice(2).ParseUInt16().Should().Be(0xABCD);
        }

        [TestMethod]
        public void ParseCopyUInt32()
        {
            byte[] raw = new byte[] { 0xFC, 0x22, 0x33, 0x44, 0x00, 0xFF, 0x78, 0x00 };
            Span<byte> buffer = new Span<byte>(raw);

            buffer.Slice(0).ParseUInt32().Should().Be(0xFC223344);
            buffer.Slice(4).ParseUInt32().Should().Be(0xFF7800U);

            0U.CopyTo(buffer.Slice(0));
            0xABCDEF11.CopyTo(buffer.Slice(4));

            raw.Should().ContainInOrder(0, 0, 0, 0, 0xAB, 0xCD, 0xEF, 0x11);
            buffer.Slice(0).ParseUInt32().Should().Be(0U);
            buffer.Slice(4).ParseUInt32().Should().Be(0xABCDEF11);
        }

        [TestMethod]
        public void ParseCopyIP()
        {
            byte[] raw = new byte[] { 0x01, 0x02, 0x03, 0x05, 0x00, 0x08, 0xA, 0x00 };
            Span<byte> buffer = new Memory<byte>(raw).Span;

            buffer.Slice(0).ParseIPAddressV4().Should().Be(new IPAddressV4(1, 2, 3, 5));
            buffer.Slice(4).ParseIPAddressV4().Should().Be(new IPAddressV4(0, 8, 10, 0));

            new IPAddressV4(7, 8, 9, 0).CopyTo(buffer.Slice(0));
            new IPAddressV4(0xC0, 0x7F, 0xFF, 0xFF).CopyTo(buffer.Slice(4));

            raw.Should().ContainInOrder(7, 8, 9, 0, 0xC0, 0x7F, 0xFF, 0xFF);
            buffer.Slice(0).ParseIPAddressV4().Should().Be(new IPAddressV4(7, 8, 9, 0));
            buffer.Slice(4).ParseIPAddressV4().Should().Be(new IPAddressV4(0xC0, 0x7F, 0xFF, 0xFF));
        }
    }
}
