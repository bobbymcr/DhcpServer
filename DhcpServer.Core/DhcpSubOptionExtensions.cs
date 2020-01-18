// <copyright file="DhcpSubOptionExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// Provides extension methods for <see cref="DhcpSubOption"/>.
    /// </summary>
    public static class DhcpSubOptionExtensions
    {
        /// <summary>
        /// Gets the sequence of RADIUS attributes from the DHCP sub-option.
        /// </summary>
        /// <param name="subOption">The RADIUS attribute sub-option.</param>
        /// <returns>The RADIUS attributes sequence.</returns>
        /// <remarks>An empty sequence is returned in the case of a mismatching sub-option.</remarks>
        public static RadiusAttribute.Sequence RadiusAttributes(this DhcpSubOption subOption)
        {
            if (subOption.Code == (byte)DhcpRelayAgentSubOptionCode.RadiusAttributes)
            {
                return new RadiusAttribute.Sequence(subOption);
            }

            return default;
        }
    }
}
