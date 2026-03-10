using System;
using System.Text;
using Callvote.API.Features.Generic;

namespace Callvote.API.Features.Pooling
{
    /// <summary>
    /// Represents the class for managing and creating <see cref="StringBuilder"/> pools.
    /// </summary>
    public class StringBuilderPool : Pool<StringBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderPool"/> class.
        /// </summary>
        /// <param name="preload">The ammount of <see cref="StringBuilder"/> to be pre-alocated.</param>
        public StringBuilderPool(int preload = 0)
            : base(() => new StringBuilder(512), preload)
        {
        }

        /// <summary>
        /// Clears and store a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="item">The <see cref="StringBuilder"/> to be stored.</param>
        public override void Store(StringBuilder item)
        {
            item.Clear();
            base.Store(item);
        }

        /// <summary>
        /// Stores and returns the <see cref="StringBuilder"/> content.
        /// </summary>
        /// <param name="item">The <see cref="StringBuilder"/> to be stored.</param>
        /// <returns>The <paramref name="item"/> text.</returns>
        public string ToStringStore(StringBuilder item)
        {
            string text = item.ToString();
            this.Store(item);
            return text;
        }
    }
}
