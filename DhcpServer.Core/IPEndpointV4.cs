// <copyright file="IPEndpointV4.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing an IPv4 network endpoint.
    /// </summary>
    public readonly struct IPEndpointV4 : IEquatable<IPEndpointV4>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IPEndpointV4"/> struct.
        /// </summary>
        /// <param name="address">The IP address.</param>
        /// <param name="port">The network port.</param>
        public IPEndpointV4(IPAddressV4 address, ushort port)
        {
            this.Address = address;
            this.Port = port;
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        public IPAddressV4 Address { get; }

        /// <summary>
        /// Gets the network port.
        /// </summary>
        public ushort Port { get; }

        /// <inheritdoc/>
        public bool Equals(IPEndpointV4 other)
        {
            return this.Address.Equals(other.Address) && (this.Port == other.Port);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode/263416#263416
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ this.Address.GetHashCode();
                hash = (hash * 16777619) ^ this.Port.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is IPEndpointV4)
            {
                return this.Equals((IPEndpointV4)obj);
            }

            return false;
        }

        /// <summary>
        /// Tries to format the current endpoint into the provided span.
        /// </summary>
        /// <param name="destination">When this method returns, the endpoint as a span of characters
        /// (for example "192.168.1.2:80").</param>
        /// <param name="charsWritten">When this method returns, the number of characters written into the span.</param>
        /// <returns><c>true </c> if the formatting was successful; otherwise, <c>false</c>.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            this.Address.TryFormat(destination, out charsWritten);
            destination[charsWritten++] = ':';
            this.Port.TryFormat(destination.Slice(charsWritten), out int c);
            charsWritten += c;
            return true;
        }
    }
}
