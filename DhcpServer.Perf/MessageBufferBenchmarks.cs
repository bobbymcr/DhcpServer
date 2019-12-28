// <copyright file="MessageBufferBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class MessageBufferBenchmarks
    {
        private byte[] raw;
        private MessageBuffer buffer;

        [GlobalSetup]
        public void Setup()
        {
            this.raw = new byte[16];
            this.buffer = new MessageBuffer(new Memory<byte>(this.raw));
        }

        [Benchmark]
        public int WriteUInt8()
        {
            int current = 0;
            this.buffer.WriteUInt8(current++, 0x01);
            this.buffer.WriteUInt8(current++, 0x02);
            this.buffer.WriteUInt8(current++, 0x04);
            this.buffer.WriteUInt8(current++, 0x08);
            this.buffer.WriteUInt8(current++, 0x10);
            this.buffer.WriteUInt8(current++, 0x20);
            this.buffer.WriteUInt8(current++, 0x40);
            this.buffer.WriteUInt8(current++, 0x80);
            this.buffer.WriteUInt8(current++, 0x01);
            this.buffer.WriteUInt8(current++, 0x02);
            this.buffer.WriteUInt8(current++, 0x04);
            this.buffer.WriteUInt8(current++, 0x08);
            this.buffer.WriteUInt8(current++, 0x10);
            this.buffer.WriteUInt8(current++, 0x20);
            this.buffer.WriteUInt8(current++, 0x40);
            this.buffer.WriteUInt8(current++, 0x80);
            return current;
        }

        [Benchmark]
        public ulong ReadUInt8()
        {
            ulong sum = 0;
            sum += this.buffer.ReadUInt8(0);
            sum += this.buffer.ReadUInt8(1);
            sum += this.buffer.ReadUInt8(2);
            sum += this.buffer.ReadUInt8(3);
            sum += this.buffer.ReadUInt8(4);
            sum += this.buffer.ReadUInt8(5);
            sum += this.buffer.ReadUInt8(6);
            sum += this.buffer.ReadUInt8(7);
            sum += this.buffer.ReadUInt8(8);
            sum += this.buffer.ReadUInt8(9);
            sum += this.buffer.ReadUInt8(10);
            sum += this.buffer.ReadUInt8(11);
            sum += this.buffer.ReadUInt8(12);
            sum += this.buffer.ReadUInt8(13);
            sum += this.buffer.ReadUInt8(14);
            sum += this.buffer.ReadUInt8(15);
            return sum;
        }

        [Benchmark]
        public int WriteUInt16()
        {
            int current = -2;
            this.buffer.WriteUInt16(current += 2, 0x0102);
            this.buffer.WriteUInt16(current += 2, 0x0408);
            this.buffer.WriteUInt16(current += 2, 0x1020);
            this.buffer.WriteUInt16(current += 2, 0x4080);
            this.buffer.WriteUInt16(current += 2, 0x0102);
            this.buffer.WriteUInt16(current += 2, 0x0408);
            this.buffer.WriteUInt16(current += 2, 0x1020);
            this.buffer.WriteUInt16(current += 2, 0x4080);
            return current;
        }

        [Benchmark]
        public ulong ReadUInt16()
        {
            ulong sum = 0;
            sum += this.buffer.ReadUInt16(0);
            sum += this.buffer.ReadUInt16(2);
            sum += this.buffer.ReadUInt16(4);
            sum += this.buffer.ReadUInt16(6);
            sum += this.buffer.ReadUInt16(8);
            sum += this.buffer.ReadUInt16(10);
            sum += this.buffer.ReadUInt16(12);
            sum += this.buffer.ReadUInt16(14);
            return sum;
        }

        [Benchmark]
        public int WriteUInt32()
        {
            int current = -4;
            this.buffer.WriteUInt32(current += 4, 0x01020408);
            this.buffer.WriteUInt32(current += 4, 0x10204080);
            this.buffer.WriteUInt32(current += 4, 0x01020408);
            this.buffer.WriteUInt32(current += 4, 0x10204080);
            return current;
        }

        [Benchmark]
        public ulong ReadUInt32()
        {
            ulong sum = 0;
            sum += this.buffer.ReadUInt32(0);
            sum += this.buffer.ReadUInt32(4);
            sum += this.buffer.ReadUInt32(8);
            sum += this.buffer.ReadUInt32(12);
            return sum;
        }
    }
}
