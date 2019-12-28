// <copyright file="SampleBenchmark.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles]
    public class SampleBenchmark
    {
        [Benchmark]
        public int Sample() => new Class1("world").ToString().Length;
    }
}
