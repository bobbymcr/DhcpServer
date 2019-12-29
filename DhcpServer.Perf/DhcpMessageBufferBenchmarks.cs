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

        private long totalSize;

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

        [Benchmark]
        public long LoadWithOptions()
        {
            this.totalSize = 0;
            this.buffer.Load(BufferSize);
            this.buffer.ReadOptions(this, (o, t) => t.totalSize += o.Data.Length);
            return this.totalSize;
        }

        [Benchmark]
        public long SaveWithOptions()
        {
            var option1 = this.buffer.WriteOption(DhcpOptionTag.DhcpMsgType, 1);
            option1.Data[0] = (byte)DhcpMessageType.Discover;
            var option2 = this.buffer.WriteOption(DhcpOptionTag.ClientId, 7);
            option2.Data[0] = (byte)DhcpHardwareAddressType.Ethernet10Mb;
            new MacAddress(0x000B8201FC42).WriteTo(option2.Data.Slice(1));
            var option3 = this.buffer.WriteOption(DhcpOptionTag.AddressRequest, 4);
            default(IPAddressV4).WriteTo(option3.Data);
            var option4 = this.buffer.WriteOption(DhcpOptionTag.ParameterList, 4);
            option4.Data[0] = (byte)DhcpOptionTag.SubnetMask;
            option4.Data[1] = (byte)DhcpOptionTag.Router;
            option4.Data[2] = (byte)DhcpOptionTag.DomainServer;
            option4.Data[3] = (byte)DhcpOptionTag.NtpServers;
            this.buffer.WriteEndOption();
            return this.buffer.Save();
        }

        [Benchmark]
        public uint Save()
        {
            this.buffer.Save();
            return this.buffer.TransactionId;
        }
    }
}
