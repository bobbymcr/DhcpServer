// <copyright file="DhcpMessageBufferBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class DhcpMessageBufferBenchmarks
    {
        private const int BufferSize = 500;

        private byte[] raw;
        private DhcpMessageBuffer buffer;

        [GlobalSetup]
        public void Setup()
        {
            this.raw = new byte[BufferSize];
            this.buffer = new DhcpMessageBuffer(new Memory<byte>(this.raw));
            PacketResource.Read("Request1", this.buffer.Span);
        }

        [Benchmark]
        public uint Load()
        {
            this.buffer.Load(BufferSize);
            return this.buffer.TransactionId;
        }
    }
}
