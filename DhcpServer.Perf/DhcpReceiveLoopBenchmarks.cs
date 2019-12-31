// <copyright file="DhcpReceiveLoopBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using DhcpServer;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class DhcpReceiveLoopBenchmarks
    {
        private IPEndpointV4 endpoint;
        private ISocket socket;
        private DhcpReceiveLoop loop;
        private byte[] rawReceiveBuffer;
        private Memory<byte> receiveBuffer;
        private Callbacks callbacks;
        private byte[] rawSendBuffer;
        private Memory<byte> sendBuffer;

        [Params(10, 100, 1000)]
        public int N { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.endpoint = new IPEndpointV4(IPAddressV4.Loopback, 67);
            this.socket = new DhcpSocket(this.endpoint);
            this.loop = new DhcpReceiveLoop(this.socket);
            this.rawReceiveBuffer = new byte[500];
            this.receiveBuffer = new Memory<byte>(this.rawReceiveBuffer);
            this.callbacks = new Callbacks();
            this.rawSendBuffer = new byte[500];
            this.sendBuffer = ReadPacket("Request1", this.rawSendBuffer);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.socket.Dispose();
        }

        [Benchmark]
        public int Run() => this.RunAsync().Result;

        private static Memory<byte> ReadPacket(string name, byte[] raw)
        {
            Memory<byte> buffer = new Memory<byte>(raw);
            int length = PacketResource.Read(name, buffer.Span);
            return buffer.Slice(0, length);
        }

        private async Task<int> RunAsync()
        {
            this.callbacks.Expect(this.N);
            Task receiveTask = this.loop.RunAsync(this.receiveBuffer, this.callbacks, CancellationToken.None);
            await this.SendAsync();
            try
            {
                await receiveTask;
            }
            catch (OperationCanceledException)
            {
                return this.callbacks.RemainingCount;
            }

            throw new NotSupportedException("Should not happen");
        }

        private async Task SendAsync()
        {
            for (int i = 0; i < this.N; ++i)
            {
                await this.socket.SendAsync(this.sendBuffer, this.endpoint);
            }
        }

        private sealed class Callbacks : IDhcpReceiveCallbacks
        {
            private static readonly Exception Canceled = new OperationCanceledException();

            public int RemainingCount { get; private set; }

            public void Expect(int count)
            {
                this.RemainingCount = count;
            }

            public ValueTask OnErrorAsync(DhcpError error, CancellationToken token)
            {
                throw new NotSupportedException("Should not happen");
            }

            public ValueTask OnReceiveAsync(DhcpMessageBuffer message, CancellationToken token)
            {
                int remaining = this.RemainingCount -= (byte)message.Opcode;
                if (remaining == 0)
                {
                    throw Canceled;
                }

                return new ValueTask(Task.CompletedTask);
            }
        }
    }
}
