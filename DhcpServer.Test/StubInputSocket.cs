// <copyright file="StubInputSocket.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class StubInputSocket : IInputSocket
    {
        private readonly Queue<Tuple<Memory<byte>, TaskCompletionSource<int>>> pending;

        public StubInputSocket()
        {
            this.pending = new Queue<Tuple<Memory<byte>, TaskCompletionSource<int>>>();
        }

        public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var next = Tuple.Create(buffer, new TaskCompletionSource<int>());
            this.pending.Enqueue(next);
            return new ValueTask<int>(next.Item2.Task);
        }

        public void Complete(string resourceName, int length = -1)
        {
            byte[] raw = new byte[66000];
            Span<byte> packet = new Memory<byte>(raw).Span;
            int actualLength = PacketResource.Read(resourceName, packet);
            if (length == -1)
            {
                length = actualLength;
            }

            this.Complete(packet.Slice(0, length));
        }

        public void Complete(ReadOnlySpan<byte> input)
        {
            var current = this.pending.Dequeue();
            Span<byte> output = current.Item1.Span;
            int safeLength = Math.Min(input.Length, output.Length);
            for (int i = 0; i < safeLength; ++i)
            {
                output[i] = input[i];
            }

            current.Item2.SetResult(input.Length);
        }

        public void Complete(Exception exception)
        {
            this.pending.Dequeue().Item2.SetException(exception);
        }
    }
}
