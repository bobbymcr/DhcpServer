// <copyright file="TimePointTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using System;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class TimePointTest
    {
        [TestMethod]
        public void ElapsedOneMillisecond()
        {
            TimePoint start = TimePoint.Now();

            Thread.Sleep(1);
            TimeSpan elapsed = start.Elapsed();

            elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromMilliseconds(1.0d));
        }
    }
}
