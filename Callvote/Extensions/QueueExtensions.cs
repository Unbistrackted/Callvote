using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Pools;

namespace Exiled.API.Extensions
{
    public static class QueueExtensions
    {
        public static void RemoveFromQueue<T>(this Queue<T> queue, int number)
        {
            List<T> list = ListPool<T>.Pool.Get();
            list = queue.ToList();
            for (int i = 0; i < queue.Count; i++)
            {
                T item = queue.Dequeue();
                if (item.Equals(list[number]))
                {
                    list.Remove(item);
                }
            }

            foreach (T item2 in list)
            {
                queue.Enqueue(item2);
            }

            ListPool<T>.Pool.Return(list);
        }
    }
}