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
        public int Mac()
        {
            return new MacAddress(0xFEDCBA987654).WriteString(new Span<char>(this.buffer));
        }

        [Benchmark]
        public int MacD()
        {
            return new MacAddress(0xFEDCBA987654).WriteString(new Span<char>(this.buffer), "D");
        }
    }
}
