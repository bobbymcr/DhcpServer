// <copyright file="Base10.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Base10
    {
        private static readonly byte[] DigitCount = new byte[256]
        {
            // 01 02 03...
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 33...
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 65...
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,

            // 97...
            2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 129...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 161...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 193...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,

            // 225...
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
        };

        public static byte UInt8DigitCount(byte value) => DigitCount[value];

        public static int WriteUInt8(Span<char> destination, int start, byte value)
        {
            if (value > 99)
            {
                WriteDigit(destination, start, (byte)(value / 100));
                WriteDigits2(destination, start + 1, (byte)(value % 100));
                return start + 3;
            }
            else if (value > 9)
            {
                WriteDigits2(destination, start, value);
                return start + 2;
            }
            else
            {
                WriteDigit(destination, start, value);
                return start + 1;
            }
        }

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
