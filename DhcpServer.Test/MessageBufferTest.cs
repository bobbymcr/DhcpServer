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
    }
}
