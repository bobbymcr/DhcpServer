// <copyright file="DhcpMessageBufferExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// Provides extension methods for <see cref="DhcpMessageBuffer"/>.
    /// </summary>
    public static class DhcpMessageBufferExtensions
    {
        /// <summary>
        /// Writes data for the subnet mask option.
        /// </summary>
        /// <param name="buffer">The message buffer.</param>
        /// <param name="mask">The subnet mask.</param>
        public static void WriteSubnetMaskOption(this DhcpMessageBuffer buffer, IPAddressV4 mask)
        {
            var option = buffer.WriteOption(DhcpOptionTag.SubnetMask, 4);
            mask.WriteTo(option.Data);
        }
    }
}
