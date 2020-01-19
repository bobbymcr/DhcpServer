// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Fuzz
{
    using System;
    using System.IO;
    using DhcpServer;
    using SharpFuzz;

    internal sealed class Program
    {
        private static Memory<byte> bytes;
        private static Memory<char> chars;
        private static Lazy<DhcpMessageBuffer> lazyBuffer;

        private static void Main()
        {
            bytes = new Memory<byte>(new byte[300]);
            chars = new Memory<char>(new char[65536]);
            lazyBuffer = new Lazy<DhcpMessageBuffer>(() => new DhcpMessageBuffer(bytes));
            Fuzzer.Run(Run);
        }

        private static void Run(Stream stream)
        {
            DhcpMessageBuffer buffer = lazyBuffer.Value;
            Span<char> destination = chars.Span;
            ushort length = (ushort)stream.Read(buffer.Span);
            buffer.Load(length);
            buffer.TryFormat(destination, out _);
            buffer.Options.TryFormat(destination, out _);
            foreach (DhcpOption option in buffer.Options)
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
