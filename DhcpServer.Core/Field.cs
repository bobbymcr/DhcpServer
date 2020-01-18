// <copyright file="Field.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Field
    {
        public delegate bool TryFormatFunc<T>(T obj, Span<char> destination, out int charsWritten);

        public static bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> field, ReadOnlySpan<char> value)
        {
            // Final result should be "<field>=<value>"
            charsWritten = 0;
            int requiredLength = field.Length + 1 + value.Length;
            if (destination.Length < requiredLength)
            {
                return false;
            }

            field.CopyTo(destination);
            destination[field.Length] = '=';
            value.CopyTo(destination.Slice(field.Length + 1));
            charsWritten = requiredLength;
            return true;
        }

        public static bool TryFormatWithNewline<T>(Span<char> destination, out int charsWritten, T obj, TryFormatFunc<T> tryFormat)
        {
            if (!tryFormat(obj, destination, out charsWritten))
            {
                return false;
            }

            bool result = TryAppend(destination.Slice(charsWritten), out int c, Environment.NewLine);
            charsWritten += c;
            return result;
        }

        public static bool TryAppend(Span<char> destination, out int charsWritten, ReadOnlySpan<char> value)
        {
            charsWritten = 0;
            if (destination.Length < value.Length)
            {
                return false;
            }

            value.CopyTo(destination);
            charsWritten = value.Length;
            return true;
        }
    }
}
