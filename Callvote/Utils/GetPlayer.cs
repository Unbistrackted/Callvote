using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callvote.Utils
{
    public static class PlayerUtils
    {
        public static Player GetPlayerByPartialName(string partialName)
        {
            try
            {
                Dictionary<string, Player> UserIdsCache = new Dictionary<string, Player>();

                foreach (Player p in Player.List)
                {
                    UserIdsCache.Add(p.UserId, p);
                }

                if (string.IsNullOrWhiteSpace(partialName))
                {
                    return null;
                }

                if (UserIdsCache.TryGetValue(partialName, out var value) && value.IsOnline)
                {
                    return value;
                }

                if (int.TryParse(partialName, out var result))
                {
                    return Player.Get(result);
                }

                if (partialName.EndsWith("@steam") || partialName.EndsWith("@discord") || partialName.EndsWith("@northwood") || partialName.EndsWith("@offline"))
                {
                    foreach (Player value2 in Player.List)
                    {
                        if (value2.UserId == partialName)
                        {
                            value = value2;
                            break;
                        }
                    }
                }
                else
                {
                    int num = 31;
                    string text = partialName.ToLower();
                    foreach (Player value3 in Player.List)
                    {
                        if (value3.IsOnline && value3.Nickname != null && value3.Nickname.ToLower().Contains(partialName.ToLower()))
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
                Logger.Warn(arg.Message);
                return null;
            }
        }
    }
}
