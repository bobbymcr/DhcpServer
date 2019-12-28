// <copyright file="Class1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    /// <summary>
    /// A sample class.
    /// </summary>
    public class Class1
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class1"/> class.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        public Class1(string name)
        {
            this.name = name;
        }

        /// <inheritdoc/>
        public override string ToString() => $"Hello, '{this.name}'!";
    }
}
