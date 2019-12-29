// <copyright file="DhcpMessageBufferTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DhcpMessageBufferTest
    {
        [TestMethod]
        public void LoadRequest()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = ReadResource("Request1.bin", buffer.Span);

            buffer.Load(length);

            buffer.Opcode.Should().Be(DhcpOpcode.Request);
        }

        [TestMethod]
        public void LoadReply()
        {
            byte[] raw = new byte[500];
            DhcpMessageBuffer buffer = new DhcpMessageBuffer(new Memory<byte>(raw));
            int length = ReadResource("Reply1.bin", buffer.Span);

            buffer.Load(length);

            buffer.Opcode.Should().Be(DhcpOpcode.Reply);
        }

        private static int ReadResource(string name, Span<byte> destination)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullName = assembly.GetManifestResourceNames().First(n => n.EndsWith("." + name));
            using Stream stream = assembly.GetManifestResourceStream(fullName);
            return stream.Read(destination);
        }
    }
}
