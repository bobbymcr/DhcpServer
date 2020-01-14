// <copyright file="DhcpSubOption.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// A value type representing a DHCP sub-option.
    /// </summary>
    /// <remarks>The END sub-option (code 255) may carry vendor-specific data
    /// in its <see cref="Data"/> span.</remarks>
    public readonly struct DhcpSubOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpSubOption"/> struct.
        /// </summary>
        /// <param name="code">The sub-option code.</param>
        /// <param name="data">The sub-option data.</param>
        public DhcpSubOption(byte code, Memory<byte> data)
        {
            this.Code = code;
            this.Data = data;
        }

        /// <summary>
        /// Gets the sub-option code.
        /// </summary>
        public byte Code { get; }

        /// <summary>
        /// Gets the sub-option data.
        /// </summary>
        public Memory<byte> Data { get; }
    }
}
