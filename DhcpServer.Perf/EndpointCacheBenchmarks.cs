// <copyright file="EndpointCacheBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System.Net;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;

    [SimpleJob(RuntimeMoniker.Net50)]
    [MemoryDiagnoser]
    public class EndpointCacheBenchmarks
    {
        [Params(256, 2048, 16384)]
        public int N { get; set; }

        [Benchmark]
        public long Cache256()
        {
            long sum = 0;
            EndpointCache cache = new EndpointCache();
            for (int i = 0; i < this.N; ++i)
            {
                IPEndpointV4 key = new IPEndpointV4(IPAddressV4.Loopback, (byte)i);
                IPEndPoint value = cache[key];
                sum += value.Port;
            }

            return sum;
        }
    }
}
