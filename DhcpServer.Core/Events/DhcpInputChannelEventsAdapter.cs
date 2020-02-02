// <copyright file="DhcpInputChannelEventsAdapter.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;

    internal sealed class DhcpInputChannelEventsAdapter : IDhcpInputChannelEvents<bool>
    {
        private readonly IDhcpInputChannelEvents inner;

        public DhcpInputChannelEventsAdapter(IDhcpInputChannelEvents inner)
        {
            this.inner = inner;
        }

        public bool ReceiveStart(DhcpChannelId id)
        {
            this.inner.ReceiveStart(id);
            return false;
        }

        public void ReceiveEnd(DhcpChannelId id, bool succeeded, DhcpError error, Exception exception, bool state)
        {
            this.inner.ReceiveEnd(id, succeeded, error, exception);
        }
    }
}