using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Callvote.Features.Extensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal static class QueueExtensions
    {
        internal static void RemoveFromQueue<T>(this ConcurrentQueue<T> queue, int index)
        {
            if (index < 0 || index >= queue.Count)
            {
                return;
            }

            for (int i = 0; i < queue.Count; i++)
            {
                if (!queue.TryDequeue(out T item))
                {
                    return;
                }

                if (i != index)
                {
                    queue.Enqueue(item);
                }
            }
        }

        internal static bool RemoveItemFromQueue<T>(this ConcurrentQueue<T> queue, T value)
        {
            bool removed = false;

            for (int i = 0; i < queue.Count; i++)
            {
                if (!queue.TryDequeue(out T item))
                {
                    return false;
                }

                if (!removed && EqualityComparer<T>.Default.Equals(item, value))
                {
                    removed = true;
                    continue;
                }

                queue.Enqueue(item);
            }

            return removed;
        }
    }
}