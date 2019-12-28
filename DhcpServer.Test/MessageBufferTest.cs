// <copyright file="MessageBufferTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MessageBufferTest
    {
        [TestMethod]
        public void ReadWriteUInt8()
        {
            byte[] raw = new byte[] { 10, 20 };
            MessageBuffer buffer = new MessageBuffer(new Memory<byte>(raw));

            buffer.ReadUInt8(0).Should().Be((byte)10);
            buffer.ReadUInt8(1).Should().Be((byte)20);

            buffer.WriteUInt8(0, 30);
            buffer.WriteUInt8(1, 40);

            raw.Should().ContainInOrder(30, 40);
            buffer.ReadUInt8(0).Should().Be((byte)30);
            buffer.ReadUInt8(1).Should().Be((byte)40);
        }

        [TestMethod]
        public void ReadWriteUInt16()
        {
            byte[] raw = new byte[] { 0xEF, 0xCD, 0x00, 0x99 };
            MessageBuffer buffer = new MessageBuffer(new Memory<byte>(raw));

            buffer.ReadUInt16(0).Should().Be((ushort)0xEFCD);
            buffer.ReadUInt16(2).Should().Be((ushort)0x0099);

            buffer.WriteUInt16(0, 0);
            buffer.WriteUInt16(2, 0xABCD);

            raw.Should().ContainInOrder(0, 0, 0xAB, 0xCD);
            buffer.ReadUInt16(0).Should().Be((ushort)0);
            buffer.ReadUInt16(2).Should().Be((ushort)0xABCD);
        }

        [TestMethod]
        public void ReadWriteUInt32()
        {
            byte[] raw = new byte[] { 0xFC, 0x22, 0x33, 0x44, 0x00, 0xFF, 0x78, 0x00 };
            MessageBuffer buffer = new MessageBuffer(new Memory<byte>(raw));

            buffer.ReadUInt32(0).Should().Be(0xFC223344);
            buffer.ReadUInt32(4).Should().Be(0xFF7800U);

            buffer.WriteUInt32(0, 0);
            buffer.WriteUInt32(4, 0xABCDEF11);

            raw.Should().ContainInOrder(0, 0, 0, 0, 0xAB, 0xCD, 0xEF, 0x11);
            buffer.ReadUInt32(0).Should().Be(0U);
            buffer.ReadUInt32(4).Should().Be(0xABCDEF11);
        }
    }
}
