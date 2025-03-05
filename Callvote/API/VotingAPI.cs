using System.Collections.Generic;
using Callvote.VoteHandlers;
using Exiled.API.Features;
using MEC;
using UserSettings.ServerSpecific;

namespace Callvote.VoteHandlers
{
    public class VotingAPI
    {
        public static Voting CurrentVoting;
        public static Dictionary<Player, int> CallvotePlayerDict = new Dictionary<Player, int>();
        public static string Vote(Player player, string option)
        {
            string playerUserId = player.UserId;
            if (VotingAPI.CurrentVoting == null) return Plugin.Instance.Translation.NoVotingInProgress;
            if (!VotingAPI.CurrentVoting.Options.ContainsKey(option)) return Plugin.Instance.Translation.NoOptionAvailable;
            if (VotingAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId))
            {
                if (VotingAPI.CurrentVoting.PlayerVote[playerUserId] == option) return Plugin.Instance.Translation.AlreadyVoted;
                VotingAPI.CurrentVoting.Counter[VotingAPI.CurrentVoting.PlayerVote[playerUserId]]--;
                VotingAPI.CurrentVoting.PlayerVote[playerUserId] = option;
            }

            if (!VotingAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId))
                VotingAPI.CurrentVoting.PlayerVote.Add(playerUserId, option);

            VotingAPI.CurrentVoting.Counter[option]++;

            return Plugin.Instance.Translation.VoteAccepted.Replace("%Reason%",
                VotingAPI.CurrentVoting.Options[option]);
        }
        public static string Rig(string argument)
        {
            if (VotingAPI.CurrentVoting == null) return "vote not active";
            if (!VotingAPI.CurrentVoting.Options.ContainsKey(argument))
                return Plugin.Instance.Translation.NoOptionAvailable;
            VotingAPI.CurrentVoting.Counter[argument]++;
            return $"Rigged LMAO {argument}";
        }

        public static IEnumerator<float> StartVotingCoroutine(Voting newVote)
        {
            int timerCounter = 0;
            VotingAPI.CurrentVoting = newVote;
            string firstBroadcast = Plugin.Instance.Translation.AskedQuestion.Replace("%Question%", VotingAPI.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
            {
                if (counter == 0)
                    firstBroadcast += $"|  {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                else
                    firstBroadcast += $"  {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";
                counter++;
            }


            int textsize = firstBroadcast.Length / 10;
            if (Plugin.Instance.Config.BroadcastSize != 0)
            {
                textsize = 52 + Plugin.Instance.Config.BroadcastSize;
            }
            Map.Broadcast(5, $"<size={52 - textsize}>{firstBroadcast}</size>");
            yield return Timing.WaitForSeconds(5f);
            for (; ; )
            {
                if (timerCounter >= Plugin.Instance.Config.VoteDuration + 1)
                {
                    if (VotingAPI.CurrentVoting.Callback == null)
                    {
                        string resultsBroadcast = Plugin.Instance.Translation.Results;
                        foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
                        {
                            resultsBroadcast += Plugin.Instance.Translation.OptionAndCounter
                                .Replace("%Option%", kvp.Value)
                                .Replace("%OptionKey%", kvp.Key)
                                .Replace("%Counter%", VotingAPI.CurrentVoting.Counter[kvp.Key].ToString());
                            textsize = resultsBroadcast.Length / 10;
                            if (Plugin.Instance.Config.BroadcastSize != 0)
                            {
                                textsize = 52 + Plugin.Instance.Config.BroadcastSize;
                            }
                        }
                        Map.Broadcast(5, $"<size={48 - textsize}>{resultsBroadcast}</size>");
                    }
                    else
                    {
                        VotingAPI.CurrentVoting.Callback.Invoke(VotingAPI.CurrentVoting);
                    }
                    CurrentVoting.Stop();
                    yield break;
                }

                {
                    string timerBroadcast = firstBroadcast + "\n";
                    foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
                    {
                        timerBroadcast += Plugin.Instance.Translation.OptionAndCounter.Replace("%Option%", kvp.Value)
                            .Replace("%OptionKey%", kvp.Key)
                            .Replace("%Counter%", VotingAPI.CurrentVoting.Counter[kvp.Key].ToString());
                        textsize = timerBroadcast.Length / 10;
                    }
                    Map.Broadcast(1, $"<size={52 - textsize}>{timerBroadcast}</size>");
                }
                timerCounter++;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingbase)
        {
            if (settingbase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == 643:
                        Vote(Player.Get(sender), Plugin.Instance.Translation.CommandYes);
                        break;

                    case int id when id == 644:
                        Vote(Player.Get(sender), Plugin.Instance.Translation.CommandNo);
                        break;

                    case int id when id == 654:
                        Vote(Player.Get(sender), Plugin.Instance.Translation.CommandMobileTaskForce);
                        break;
                    case int id when id == 656:
                        Vote(Player.Get(sender), Plugin.Instance.Translation.CommandChaosInsurgency);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

public delegate void CallvoteFunction(Voting vote);