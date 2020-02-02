// <copyright file="DhcpReceiveLoopBenchmarks.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Perf
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Threading;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using DhcpServer;
    using DhcpServer.Events;

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    public class DhcpReceiveLoopBenchmarks
    {
        private IPEndpointV4 endpoint;
        private ISocket socket;
        private ISocket socketEvt;
        private DhcpReceiveLoop loop;
        private DhcpReceiveLoop loopEvt;
        private byte[] rawReceiveBuffer;
        private Memory<byte> receiveBuffer;
        private Callbacks callbacks;
        private byte[] rawSendBuffer;
        private Memory<byte> sendBuffer;

        [Params(1000)]
        public int N { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.endpoint = new IPEndpointV4(IPAddressV4.Loopback, 67);
            this.socket = new DhcpSocket(this.endpoint);
            this.socketEvt = this.socket.WithEvents(1, SampleEventSource.Instance);
            this.loop = new DhcpReceiveLoop(new DhcpInputChannelFactory(this.socket));
            IDhcpInputChannelFactory factoryEvt = new DhcpInputChannelFactory(this.socketEvt)
                .WithEvents(SampleEventSource.Instance, SampleEventSource.Instance);
            this.loopEvt = new DhcpReceiveLoop(factoryEvt);
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
        public int Run() => this.RunAsync(this.loop, this.socket).Result;

        [Benchmark]
        public int RunEvent() => this.RunAsync(this.loopEvt, this.socketEvt).Result;

        private static Memory<byte> ReadPacket(string name, byte[] raw)
        {
            Memory<byte> buffer = new Memory<byte>(raw);
            int length = PacketResource.Read(name, buffer.Span);
            return buffer.Slice(0, length);
        }

        private async Task<int> RunAsync(DhcpReceiveLoop receive, IOutputSocket output)
        {
            this.callbacks.Expect(this.N);
            Task receiveTask = receive.RunAsync(this.receiveBuffer, this.callbacks, CancellationToken.None);
            await this.SendAsync(output);
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

        private async Task SendAsync(IOutputSocket output)
        {
            // Send slightly more than the expected number of messages
            // in case some get dropped.
            int n = (int)(this.N * 1.15);
            for (int i = 0; i < n; ++i)
            {
                await output.SendAsync(this.sendBuffer, this.endpoint);
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

        private sealed class SampleEventSource : IDhcpInputChannelFactoryEvents, IDhcpInputChannelEvents, ISocketEvents
        {
            public static readonly SampleEventSource Instance = new SampleEventSource();

            private SampleEventSource()
            {
            }

            [NonEvent]
            public void CreateChannelStart(DhcpChannelId id, int bufferSize)
            {
            }

            [NonEvent]
            public void CreateChannelEnd(DhcpChannelId id, bool succeeded, Exception exception)
            {
            }

            [NonEvent]
            public void ReceiveStart(DhcpChannelId id)
            {
            }

            [NonEvent]
            public void ReceiveEnd(DhcpChannelId id, bool succeeded, DhcpError error, Exception exception)
            {
            }

            [NonEvent]
            public void DisposeStart(SocketId id)
            {
            }

            [NonEvent]
            public void DisposeEnd(SocketId id)
            {
            }

            [NonEvent]
            public void ReceiveStart(SocketId id, int bufferSize)
            {
            }

            [NonEvent]
            public void ReceiveEnd(SocketId id, int result, bool succeeded, Exception exception)
            {
            }

            [NonEvent]
            public void SendStart(SocketId id, int bufferSize, IPEndpointV4 endpoint)
            {
            }

            [NonEvent]
            public void SendEnd(SocketId id, bool succeeded, Exception exception)
            {
            }
        }
    }
}
