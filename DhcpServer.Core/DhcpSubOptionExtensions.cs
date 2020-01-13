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
        public static RadiusAttribute.Sequence RadiusAttributes(this DhcpSubOption subOption)
        {
            return new RadiusAttribute.Sequence(subOption);
        }
    }
}
