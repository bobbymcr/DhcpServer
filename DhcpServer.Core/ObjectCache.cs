// <copyright file="ObjectCache.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Holds a cache of keyed objects.
    /// </summary>
    /// <typeparam name="TKey">The type for the object key.</typeparam>
    /// <typeparam name="TValue">The type for the object value.</typeparam>
    public abstract class ObjectCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCache{TInput, TOutput}"/> class.
        /// </summary>
        protected ObjectCache()
        {
            this.cache = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets a cached object instance.
        /// </summary>
        /// <remarks>If the value is not yet cached, this method will create a new instance,
        /// cache it, and return it.</remarks>
        /// <param name="key">The object key.</param>
        /// <returns>The object value.</returns>
        public TValue this[TKey key] => this.cache.GetOrAdd(key, (k, t) => t.Create(k), this);

        /// <summary>
        /// Creates a new object instance from the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>The newly created object value.</returns>
        protected abstract TValue Create(TKey key);
    }
}
