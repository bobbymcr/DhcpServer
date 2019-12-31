// <copyright file="EndpointCache.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System.Net;

    /// <summary>
    /// Caches <see cref="IPEndPoint"/> instances.
    /// </summary>
    public sealed class EndpointCache : ObjectCache<IPEndpointV4, IPEndPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointCache"/> class.
        /// </summary>
        public EndpointCache()
        {
        }

        /// <inheritdoc/>
        protected override IPEndPoint Create(IPEndpointV4 key) => new IPEndPoint((uint)key.Address, key.Port);
    }
}
