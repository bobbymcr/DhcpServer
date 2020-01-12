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

        [Benchmark]
        public long LoadWithOptions()
        {
            long totalSize = 0;
            this.buffer.Load(BufferSize);
            foreach (DhcpOption option in this.buffer.Options)
            {
                totalSize += option.Data.Length;
            }

            return totalSize;
        }

        [Benchmark]
        public long SaveWithOptions()
        {
            this.buffer.WriteDhcpMsgTypeOption(DhcpMessageType.Discover);
            var option2 = this.buffer.WriteOptionHeader(DhcpOptionTag.ClientId, 7);
            option2.Data[0] = (byte)DhcpHardwareAddressType.Ethernet10Mb;
            new MacAddress(0x000B8201FC42).CopyTo(option2.Data.Slice(1));
            var option3 = this.buffer.WriteOptionHeader(DhcpOptionTag.AddressRequest, 4);
            default(IPAddressV4).CopyTo(option3.Data);
            this.buffer.WriteParameterListOption(
                DhcpOptionTag.SubnetMask,
                DhcpOptionTag.Router,
                DhcpOptionTag.DomainServer,
                DhcpOptionTag.NtpServers);
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
