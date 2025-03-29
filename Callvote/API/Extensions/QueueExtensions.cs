using System.Collections.Generic;
using System.Linq;

namespace Exiled.API.Extensions
{
    public static class QueueExtensions
    {
        public static void RemoveFromQueue<T>(this Queue<T> queue, int index)
        {
            if (index < 0 || index >= queue.Count) { return; }

            T element = queue.ElementAt(index);

            List<T> list = queue.Where(item => !item.Equals(element)).ToList();

            queue.Clear();

            foreach (T item2 in list)
            {
                queue.Enqueue(item2);
            }
        }

        public static void RemoveFromQueuePatch<T>(this Queue<T> queue, T value)
        {
            List<T> list = queue.Where(item => !item.Equals(value)).ToList();

            queue.Clear();

            foreach (T item2 in list)
            {
                queue.Enqueue(item2);
            }
        }
    }
}