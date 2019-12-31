// <copyright file="ObjectCacheTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Test
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class ObjectCacheTest
    {
        [TestMethod]
        public void Basic()
        {
            StringCache cache = new StringCache();

            string a1 = cache['a'];
            string b = cache['b'];
            string c = cache['c'];
            string a2 = cache['a'];

            a1.Should().Be("a");
            b.Should().Be("b");
            c.Should().Be("c");
            a2.Should().BeSameAs(a1);
        }

        private sealed class StringCache : ObjectCache<char, string>
        {
            protected override string Create(char input) => input.ToString();
        }
    }
}
