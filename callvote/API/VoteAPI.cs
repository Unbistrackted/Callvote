using System.Collections.Generic;
using Callvote.Commands;
using Callvote.VoteHandlers;
using Exiled.API.Features;
using Exiled.Events.Patches.Events.Scp106;

using MEC;
using PlayerRoles;
using RemoteAdmin;
using UserSettings.ServerSpecific;

namespace Callvote.VoteHandlers
{
    public class VoteAPI
    {
        public static Voting CurrentVoting;
        public static string Voting(Player player, string option)
        {
            string playerUserId = player.UserId;
            if (VoteAPI.CurrentVoting == null) return Plugin.Instance.Translation.NoVotingInProgress;
            if (!VoteAPI.CurrentVoting.Options.ContainsKey(option)) return Plugin.Instance.Translation.NoOptionAvailable;
            if (VoteAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId))
            {
                if (VoteAPI.CurrentVoting.PlayerVote[playerUserId] == option) return Plugin.Instance.Translation.AlreadyVoted;
                VoteAPI.CurrentVoting.Counter[VoteAPI.CurrentVoting.PlayerVote[playerUserId]]--;
                VoteAPI.CurrentVoting.PlayerVote[playerUserId] = option;
            }

            if (!VoteAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId))
                VoteAPI.CurrentVoting.PlayerVote.Add(playerUserId, option);

            VoteAPI.CurrentVoting.Counter[option]++;

            return Plugin.Instance.Translation.VoteAccepted.Replace("%Reason%",
                VoteAPI.CurrentVoting.Options[option]);
        }

        public static IEnumerator<float> StartVoteCoroutine(Voting newVote)
        {
            int timerCounter = 0;
            VoteAPI.CurrentVoting = newVote;
            string firstBroadcast = Plugin.Instance.Translation.AskedQuestion.Replace("%Question%", VoteAPI.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VoteAPI.CurrentVoting.Options)
            {
                if (counter == VoteAPI.CurrentVoting.Options.Count - 1)
                    firstBroadcast += $", {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}";
                else
                    firstBroadcast += $" {Plugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}";
                counter++;
            }

            int textsize = firstBroadcast.Length / 10;
            Map.Broadcast(5, "<size=" + (48 - textsize) + ">" + firstBroadcast + "</size>");
            yield return Timing.WaitForSeconds(5f);
            for (; ; )
            {
                if (timerCounter >= Plugin.Instance.Config.VoteDuration + 1)
                {
                    if (VoteAPI.CurrentVoting.Callback == null)
                    {
                        string timerBroadcast = Plugin.Instance.Translation.Results;
                        foreach (KeyValuePair<string, string> kvp in VoteAPI.CurrentVoting.Options)
                        {
                            timerBroadcast += Plugin.Instance.Translation.OptionAndCounter
                                .Replace("%Option%", kvp.Value)
                                .Replace("%OptionKey%", kvp.Key).Replace("%Counter%", VoteAPI.CurrentVoting.Counter[kvp.Key].ToString());
                            textsize = timerBroadcast.Length / 10;
                        }

                        Map.Broadcast(5, $"<size={48 - textsize}>{timerBroadcast}</size>");
                    }
                    else
                    {
                        VoteAPI.CurrentVoting.Callback.Invoke(VoteAPI.CurrentVoting);
                    }

                    CurrentVoting.Stop();
                    yield break;
                }

                {
                    string timerBroadcast = firstBroadcast + "\n";
                    foreach (KeyValuePair<string, string> kvp in VoteAPI.CurrentVoting.Options)
                    {
                        timerBroadcast += Plugin.Instance.Translation.OptionAndCounter.Replace("%Option%", kvp.Value)
                            .Replace("%OptionKey%", kvp.Key)
                            .Replace("%Counter%", VoteAPI.CurrentVoting.Counter[kvp.Key].ToString());
                        textsize = timerBroadcast.Length / 10;
                    }

                    Map.Broadcast(1, $"<size={48 - textsize}>{timerBroadcast}</size>");
                }
                timerCounter++;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public static string Rigging(string argument)
        {
            if (VoteAPI.CurrentVoting == null) return "vote not active";
            if (!VoteAPI.CurrentVoting.Options.ContainsKey(argument))
                return Plugin.Instance.Translation.NoOptionAvailable;
            VoteAPI.CurrentVoting.Counter[argument]++;
            return $"Rigged LMAO {argument}";
        }
        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingbase)
        {
            if (CurrentVoting == null)
                return;

            if (settingbase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch ((int)keybindSetting.SettingId)
                {
                    case int id when id == 501:
                        Voting(Player.Get(sender), Plugin.Instance.Translation.CommandYes);
                        break;

                    case int id when id == 502:
                        Voting(Player.Get(sender), Plugin.Instance.Translation.CommandNo);
                        break;

                    case int id when id == 503:
                        Voting(Player.Get(sender), Plugin.Instance.Translation.CommandMobileTaskForce);
                        break;
                    case int id when id == 504:
                        Voting(Player.Get(sender), Plugin.Instance.Translation.CommandChaosInsurgency);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ApplyCallvoteMenu(Player player)
        {
            ServerSpecificSettingsSync.DefinedSettings = SettingHandlers.CallvoteMenu();
            ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);
        }
    }
}

public delegate void CallvoteFunction(Voting vote);