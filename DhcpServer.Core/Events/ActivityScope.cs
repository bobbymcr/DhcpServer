// <copyright file="ActivityScope.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// Sets the current activity ID and restores the previous value on <see cref="Dispose"/>.
    /// </summary>
    public readonly struct ActivityScope : IDisposable
    {
        private readonly Guid previous;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityScope"/> struct.
        /// </summary>
        /// <param name="id">The new activity ID.</param>
        public ActivityScope(Guid id)
        {
            EventSource.SetCurrentThreadActivityId(id, out this.previous);
        }

        /// <summary>
        /// Gets the current activity ID.
        /// </summary>
        public static Guid CurrentId => EventSource.CurrentThreadActivityId;

        /// <inheritdoc/>
        public void Dispose()
        {
            EventSource.SetCurrentThreadActivityId(this.previous);
        }
    }
}
