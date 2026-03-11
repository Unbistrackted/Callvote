using System;
using System.Collections.Concurrent;

namespace Callvote.API.Features.Generic
{
    /// <summary>
    /// Represents the class for managing and creating pools.
    /// </summary>
    /// <typeparam name="T">The object to be pooled.</typeparam>
    public class Pool<T> : IDisposable
    {
        private readonly ConcurrentBag<T> pool;

        private readonly Func<T> factory;

        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}"/> class.
        /// </summary>
        /// <param name="factory">The item factory.</param>
        /// <param name="preload">The ammount of items to preload.</param>
        public Pool(Func<T> factory, int preload = 0)
        {
            this.factory = factory;
            this.pool = new ConcurrentBag<T>();
            this.PreloadItems(preload);
        }

        /// <summary>
        /// Fetchs an item from the ppol.
        /// </summary>
        /// <returns>The item to be used.</returns>
        public virtual T Fetch() => this.pool.TryTake(out T item) ? item : this.factory();

        /// <summary>
        /// Stores an item into the pool.
        /// </summary>
        /// <param name="item">The item to be stored.</param>
        public virtual void Store(T item) => this.pool.Add(item);

        /// <summary>
        /// Pre-alocates a specific ammount of itens into the pool.
        /// </summary>
        /// <param name="ammount">The ammount to be pre-alocated.</param>
        public void PreloadItems(int ammount)
        {
            for (int i = 0; i < ammount; i++)
            {
                this.Store(this.factory());
            }
        }

        /// <summary>
        /// Disposes the pool.
        /// </summary>
        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;

            if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                lock (this.pool)
                {
                    while (this.pool.Count > 0)
                    {
                        IDisposable disposable = (IDisposable)this.Fetch();
                        disposable.Dispose();
                    }
                }
            }
        }
    }
}
