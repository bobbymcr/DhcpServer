// <copyright file="Base10.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Base10
    {
        public static void FormatDigit(Span<char> destination, int start, byte d)
        {
            destination[start] = (char)(d + '0');
        }

        public static void FormatDigits2(Span<char> destination, int start, byte dd)
        {
            FormatDigit(destination, start, (byte)(dd / 10));
            FormatDigit(destination, start + 1, (byte)(dd % 10));
        }

        public static void FormatDigits3(Span<char> destination, int start, ushort ddd)
        {
            FormatDigit(destination, start, (byte)(ddd / 100));
            FormatDigits2(destination, start + 1, (byte)(ddd % 100));
        }

        public static void FormatDigits4(Span<char> destination, int start, ushort dddd)
        {
            FormatDigit(destination, start, (byte)(dddd / 1000));
            FormatDigits3(destination, start + 1, (ushort)(dddd % 1000));
        }

        public static void FormatDigits5(Span<char> destination, int start, ushort ddddd)
        {
            FormatDigit(destination, start, (byte)(ddddd / 10000));
            FormatDigits4(destination, start + 1, (ushort)(ddddd % 10000));
        }
    }
}
