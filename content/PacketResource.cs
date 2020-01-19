// <copyright file="PacketResource.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal static class PacketResource
    {
        public static ushort Read(string name, Span<byte> destination)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullName = assembly.GetManifestResourceNames().First(n => n.EndsWith("." + name + ".bin"));
            using Stream stream = assembly.GetManifestResourceStream(fullName);
            return (ushort)stream.Read(destination);
        }
    }
}
