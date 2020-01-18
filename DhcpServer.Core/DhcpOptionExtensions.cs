﻿// <copyright file="DhcpOptionExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// Provides extension methods for <see cref="DhcpOption"/>.
    /// </summary>
    public static class DhcpOptionExtensions
    {
        /// <summary>
        /// Gets the sequence of DHCP relay agent information from the DHCP option.
        /// </summary>
        /// <param name="option">The relay agent information DHCP option.</param>
        /// <returns>The DHCP relay agent information sequence.</returns>
        public static DhcpRelayAgentInformationSubOption.Sequence RelayAgentInformation(this DhcpOption option)
        {
            return new DhcpRelayAgentInformationSubOption.Sequence(option);
        }
    }
}
