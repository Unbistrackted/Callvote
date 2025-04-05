using GameCore;
using LabApi.Features.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callvote.API.Extensions
{
    public static class PlayerExtension
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
        public static Player Get(this Player playert, string args)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(args))
                {
                    return null;
                }

                if (Player.List..TryGetValue(args, out var value) && value.IsConnected)
                {
                    return value;
                }

                if (int.TryParse(args, out var result))
                {
                    return Get(result);
                }

                if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood") || args.EndsWith("@offline"))
                {
                    foreach (Player value2 in Dictionary.Values)
                    {
                        if (value2.UserId == args)
                        {
                            value = value2;
                            break;
                        }
                    }
                }
                else
                {
                    int num = 31;
                    string text = args.ToLower();
                    foreach (Player value3 in Dictionary.Values)
                    {
                        if (value3.IsOffline && value3.Nickname != null && value3.Nickname.ToLower().Contains(args.ToLower()))
                        {
                            int num2 = value3.Nickname.Length - text.Length;
                            if (num2 < num)
                            {
                                num = num2;
                                value = value3;
                            }
                        }
                    }
                }

                if ((object)value != null)
                {
                    UserIdsCache[value.UserId] = value;
                }

                return value;
            }
            catch (Exception arg)
            {
                return null;
            }
        }
    }
}
