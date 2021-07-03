// <copyright file="TimePointBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System.Diagnostics;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    public class TimePointBenchmarks
    {
        [Benchmark]
        public long Watch()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            return stopwatch.Elapsed.Ticks;
        }

        [Benchmark]
        public long Point()
        {
            TimePoint start = TimePoint.Now();
            return start.Elapsed().Ticks;
        }
    }
}
