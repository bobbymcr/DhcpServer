// <copyright file="DhcpRelayAgentSubOptionsBuffer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Text;

    /// <summary>
    /// Provides write access to a buffer containing DHCP relay agent sub-options.
    /// </summary>
    public readonly struct DhcpRelayAgentSubOptionsBuffer
    {
        private readonly DhcpMessageBuffer buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpRelayAgentSubOptionsBuffer"/> struct
        /// and writes the relay agent information option header to the buffer.
        /// </summary>
        /// <param name="buffer">The underlying message buffer.</param>
        public DhcpRelayAgentSubOptionsBuffer(DhcpMessageBuffer buffer)
        {
            this.buffer = buffer;
            this.buffer.WriteContainerOptionHeader(DhcpOptionTag.RelayAgentInformation);
        }

        /// <summary>
        /// Writes the agent circuit ID sub-option.
        /// </summary>
        /// <param name="id">The agent circuit ID buffer.</param>
        public void WriteAgentCircuitId(ReadOnlySpan<char> id)
        {
            this.WriteAscii(DhcpRelayAgentSubOptionCode.AgentCircuitId, id);
        }

        /// <summary>
        /// Writes the agent remote ID sub-option.
        /// </summary>
        /// <param name="id">The agent remote ID buffer.</param>
        public void WriteAgentRemoteId(ReadOnlySpan<char> id)
        {
            this.WriteAscii(DhcpRelayAgentSubOptionCode.AgentRemoteId, id);
        }

        /// <summary>
        /// Writes the link selection sub-option.
        /// </summary>
        /// <param name="subnet">The subnet address.</param>
        public void WriteLinkSelection(IPAddressV4 subnet)
        {
            var option = this.buffer.WriteSubOptionHeader((byte)DhcpRelayAgentSubOptionCode.LinkSelection, 4);
            subnet.WriteTo(option.Data);
        }

        /// <summary>
        /// Writes the subscriber ID sub-option.
        /// </summary>
        /// <param name="id">The subscriber ID buffer.</param>
        public void WriteSubscriberId(ReadOnlySpan<char> id)
        {
            this.WriteAscii(DhcpRelayAgentSubOptionCode.SubscriberId, id);
        }

        /// <summary>
        /// Marks the end of the relay agent information option.
        /// </summary>
        public void End() => this.buffer.EndContainerOption();

        private void WriteAscii(DhcpRelayAgentSubOptionCode code, ReadOnlySpan<char> chars)
        {
            this.buffer.WriteSubOption((byte)code, chars, Encoding.ASCII);
        }
    }
}
