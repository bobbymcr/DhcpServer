// <copyright file="EqualityTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using FluentAssertions;

    internal static class EqualityTest
    {
        public static void Check<T>(bool areEqual, T x, T y)
            where T : IEquatable<T>
        {
            x.Equals(y).Should().Be(areEqual);
            y.Equals(x).Should().Be(areEqual);
            ((object)x).Equals(y).Should().Be(areEqual);
            y.Equals((object)x).Should().Be(areEqual);
        }
    }
}
