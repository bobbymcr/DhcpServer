// <copyright file="Field.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;

    internal static class Field
    {
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
