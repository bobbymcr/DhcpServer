// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using BenchmarkDotNet.Running;

    internal sealed class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<DhcpMessageBufferBenchmarks>();
        }
    }
}
