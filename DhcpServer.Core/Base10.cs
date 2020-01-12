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

        public static byte UInt16DigitCount(ushort value)
        {
            if (value > 9999)
            {
                return 5;
            }
            else if (value > 999)
            {
                return 4;
            }
            else if (value > 99)
            {
                return 3;
            }
            else if (value > 9)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

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

        private static void WriteDigit(Span<char> destination, int start, byte d)
        {
            destination[start] = (char)(d + '0');
        }

        private static void WriteDigits2(Span<char> destination, int start, byte dd)
        {
            WriteDigit(destination, start, (byte)(dd / 10));
            WriteDigit(destination, start + 1, (byte)(dd % 10));
        }
    }
}
