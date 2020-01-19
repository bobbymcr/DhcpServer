// <copyright file="FuzzTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using DhcpServer;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class FuzzTest
    {
        [TestMethod]
        public void BadInputs()
        {
            Test("Fuzz001", 300);
        }

        private static void Test(string name, int size)
        {
            FuzzedBuffer buffer = new FuzzedBuffer(name, size);

            buffer.ExecutionTimeOf(b => b.Run())
                .Should().BeLessThan(TimeSpan.FromSeconds(5.0d), because: "{0} should not hang", name);
        }

        private sealed class FuzzedBuffer
        {
            private readonly DhcpMessageBuffer buffer;

            public FuzzedBuffer(string name, int size)
            {
                this.buffer = new DhcpMessageBuffer(new Memory<byte>(new byte[size]));
                ushort length = PacketResource.Read(name, this.buffer.Span);
                this.buffer.Load(length);
            }

            public void Run()
            {
                Span<char> destination = new Span<char>(new char[65536]);
                this.buffer.TryFormat(destination, out _);
                this.buffer.Options.TryFormat(destination, out _);
                foreach (DhcpOption option in this.buffer.Options)
                {
                    option.RelayAgentInformation().TryFormat(destination, out _);
                    option.SubOptions.TryFormat(destination, out _);
                    foreach (DhcpSubOption subOption in option.SubOptions)
                    {
                        subOption.RadiusAttributes().TryFormat(destination, out _);
                    }
                }
            }
        }
    }
}