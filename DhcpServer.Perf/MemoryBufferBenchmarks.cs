// <copyright file="MemoryBufferBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class MemoryBufferBenchmarks
    {
        private byte[] raw;
        private Memory<byte> buffer;

        [GlobalSetup]
        public void Setup()
        {
            this.raw = new byte[16];
            this.buffer = new Memory<byte>(this.raw);
        }

        [Benchmark]
        public int CopyUInt8()
        {
            int current = 0;
            ((byte)0x01).CopyTo(this.buffer, current++);
            ((byte)0x02).CopyTo(this.buffer, current++);
            ((byte)0x04).CopyTo(this.buffer, current++);
            ((byte)0x08).CopyTo(this.buffer, current++);
            ((byte)0x10).CopyTo(this.buffer, current++);
            ((byte)0x20).CopyTo(this.buffer, current++);
            ((byte)0x40).CopyTo(this.buffer, current++);
            ((byte)0x80).CopyTo(this.buffer, current++);
            ((byte)0x01).CopyTo(this.buffer, current++);
            ((byte)0x02).CopyTo(this.buffer, current++);
            ((byte)0x04).CopyTo(this.buffer, current++);
            ((byte)0x08).CopyTo(this.buffer, current++);
            ((byte)0x10).CopyTo(this.buffer, current++);
            ((byte)0x20).CopyTo(this.buffer, current++);
            ((byte)0x40).CopyTo(this.buffer, current++);
            ((byte)0x80).CopyTo(this.buffer, current++);
            return current;
        }

        [Benchmark]
        public ulong ParseUInt8()
        {
            ulong sum = 0;
            sum += this.buffer.ParseUInt8(0);
            sum += this.buffer.ParseUInt8(1);
            sum += this.buffer.ParseUInt8(2);
            sum += this.buffer.ParseUInt8(3);
            sum += this.buffer.ParseUInt8(4);
            sum += this.buffer.ParseUInt8(5);
            sum += this.buffer.ParseUInt8(6);
            sum += this.buffer.ParseUInt8(7);
            sum += this.buffer.ParseUInt8(8);
            sum += this.buffer.ParseUInt8(9);
            sum += this.buffer.ParseUInt8(10);
            sum += this.buffer.ParseUInt8(11);
            sum += this.buffer.ParseUInt8(12);
            sum += this.buffer.ParseUInt8(13);
            sum += this.buffer.ParseUInt8(14);
            sum += this.buffer.ParseUInt8(15);
            return sum;
        }

        [Benchmark]
        public int CopyUInt16()
        {
            int current = -2;
            ((ushort)0x0102).CopyTo(this.buffer, current += 2);
            ((ushort)0x0408).CopyTo(this.buffer, current += 2);
            ((ushort)0x1020).CopyTo(this.buffer, current += 2);
            ((ushort)0x4080).CopyTo(this.buffer, current += 2);
            ((ushort)0x0102).CopyTo(this.buffer, current += 2);
            ((ushort)0x0408).CopyTo(this.buffer, current += 2);
            ((ushort)0x1020).CopyTo(this.buffer, current += 2);
            ((ushort)0x4080).CopyTo(this.buffer, current += 2);
            return current;
        }

        [Benchmark]
        public ulong ParseUInt16()
        {
            ulong sum = 0;
            sum += this.buffer.ParseUInt16(0);
            sum += this.buffer.ParseUInt16(2);
            sum += this.buffer.ParseUInt16(4);
            sum += this.buffer.ParseUInt16(6);
            sum += this.buffer.ParseUInt16(8);
            sum += this.buffer.ParseUInt16(10);
            sum += this.buffer.ParseUInt16(12);
            sum += this.buffer.ParseUInt16(14);
            return sum;
        }

        [Benchmark]
        public int CopyUInt32()
        {
            int current = -4;
            0x01020408U.CopyTo(this.buffer, current += 4);
            0x10204080U.CopyTo(this.buffer, current += 4);
            0x01020408U.CopyTo(this.buffer, current += 4);
            0x10204080U.CopyTo(this.buffer, current += 4);
            return current;
        }

        [Benchmark]
        public ulong ParseUInt32()
        {
            ulong sum = 0;
            sum += this.buffer.ParseUInt32(0);
            sum += this.buffer.ParseUInt32(4);
            sum += this.buffer.ParseUInt32(8);
            sum += this.buffer.ParseUInt32(12);
            return sum;
        }
    }
}
