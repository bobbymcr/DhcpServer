// <copyright file="StringBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class StringBenchmarks
    {
        private char[] buffer;

        [GlobalSetup]
        public void Setup()
        {
            this.buffer = new char[32];
        }

        [Benchmark]
        public int IP()
        {
            new IPAddressV4(0xC0A8000F).TryFormat(new Span<char>(this.buffer), out int c);
            return c;
        }

        [Benchmark]
        public int Mac()
        {
            new MacAddress(0xFEDCBA987654).TryFormat(new Span<char>(this.buffer), out int c);
            return c;
        }

        [Benchmark]
        public int MacEmpty()
        {
            new MacAddress(0xFEDCBA987654).TryFormat(new Span<char>(this.buffer), out int c, string.Empty);
            return c;
        }

        [Benchmark]
        public int MacNull()
        {
            new MacAddress(0xFEDCBA987654).TryFormat(new Span<char>(this.buffer), out int c, null);
            return c;
        }

        [Benchmark]
        public int MacD()
        {
            new MacAddress(0xFEDCBA987654).TryFormat(new Span<char>(this.buffer), out int c, "D");
            return c;
        }

        [Benchmark]
        public int MacN()
        {
            new MacAddress(0xFEDCBA987654).TryFormat(new Span<char>(this.buffer), out int c, "N");
            return c;
        }
    }
}
