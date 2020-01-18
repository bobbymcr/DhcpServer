// <copyright file="Hex.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Hex
    {
        public static void Format(Span<char> destination, int start, byte xx)
        {
            destination[start] = Digit((byte)((xx >> 4) & 0xF));
            destination[start + 1] = Digit((byte)(xx & 0xF));
        }

        private static char Digit(byte d)
        {
            switch (d)
            {
                case 0x0:
                case 0x1:
                case 0x2:
                case 0x3:
                case 0x4:
                case 0x5:
                case 0x6:
                case 0x7:
                case 0x8:
                case 0x9:
                    return (char)(d + '0');
                default:
                    return (char)(d - 0xA + 'A');
            }
        }
    }
}
