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
        public void CopyUInt32()
        {
            byte[] raw = new byte[] { 0xFC, 0x22, 0x33, 0x44, 0x00, 0xFF, 0x78, 0x00 };
            Span<byte> buffer = new Span<byte>(raw);

            0U.CopyTo(buffer.Slice(0));
            0xABCDEF11.CopyTo(buffer.Slice(4));

            raw.Should().ContainInOrder(0, 0, 0, 0, 0xAB, 0xCD, 0xEF, 0x11);
        }
    }
}
