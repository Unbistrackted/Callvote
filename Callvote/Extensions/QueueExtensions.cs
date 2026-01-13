using System.Collections.Generic;

namespace Callvote.Extensions
{
    public static class QueueExtensions
    {
        public static void RemoveFromQueue<T>(this Queue<T> queue, int index)
        {
            if (index < 0 || index >= queue.Count)  
                return;

            for (int i = 0; i < queue.Count; i++)
            {
                T item = queue.Dequeue();

                if (i != index)
                    queue.Enqueue(item);
            }
        }

        public static bool RemoveItemFromQueue<T>(this Queue<T> queue, T value)
        {
            bool removed = false;

            for (int i = 0; i < queue.Count; i++)
            {
                T item = queue.Dequeue();

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