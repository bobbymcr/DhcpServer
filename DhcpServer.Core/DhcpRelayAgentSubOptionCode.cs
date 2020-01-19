// <copyright file="DhcpRelayAgentSubOptionCode.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// The DHCP relay agent sub-option code, as defined in
    /// <see href="https://www.iana.org/assignments/bootp-dhcp-parameters/bootp-dhcp-parameters.xhtml#relay-agent-sub-options"/> .
    /// </summary>
    public enum DhcpRelayAgentSubOptionCode : byte
    {
        /// <summary>
        /// Unspecified sub-option
        /// </summary>
        None = 0,

        /// <summary>
        /// Agent circuit ID
        /// </summary>
        AgentCircuitId = 1,

        /// <summary>
        /// Agent remote ID
        /// </summary>
        AgentRemoteId = 2,

        /// <summary>
        /// DOCSIS device class
        /// </summary>
        DocsisDeviceClass = 4,

        /// <summary>
        /// Link selection
        /// </summary>
        LinkSelection = 5,

        /// <summary>
        /// Subscriber ID
        /// </summary>
        SubscriberId = 6,

        /// <summary>
        /// RADIUS attributes
        /// </summary>
        RadiusAttributes = 7,
    }
}
