// <copyright file="Base10.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Base10
    {
        public static void WriteDigit(Span<char> destination, int start, byte d)
        {
            destination[start] = (char)(d + '0');
        }

        public static void WriteDigits2(Span<char> destination, int start, byte dd)
        {
            WriteDigit(destination, start, (byte)(dd / 10));
            WriteDigit(destination, start + 1, (byte)(dd % 10));
        }

        public static void WriteDigits3(Span<char> destination, int start, ushort ddd)
        {
            WriteDigit(destination, start, (byte)(ddd / 100));
            WriteDigits2(destination, start + 1, (byte)(ddd % 100));
        }

        public static void WriteDigits4(Span<char> destination, int start, ushort dddd)
        {
            WriteDigit(destination, start, (byte)(dddd / 1000));
            WriteDigits3(destination, start + 1, (ushort)(dddd % 1000));
        }

        public static void WriteDigits5(Span<char> destination, int start, ushort ddddd)
        {
            WriteDigit(destination, start, (byte)(ddddd / 10000));
            WriteDigits4(destination, start + 1, (ushort)(ddddd % 10000));
        }
    }
}
