// <copyright file="ISocket.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    /// <summary>
    /// Represents a datagram input/output socket.
    /// </summary>
    public interface ISocket : IInputSocket, IOutputSocket, IDisposable
    {
    }
}
