﻿using System.Collections.Generic;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using UserSettings.ServerSpecific;

namespace Callvote.VoteHandlers
{
    public class VotingAPI
    {
        public static Voting CurrentVoting;
        public static Dictionary<Player, int> PlayerCallVotingAmount = new Dictionary<Player, int>();
        public static Dictionary<string, string> Options = new Dictionary<string, string>();
        public static string Vote(Player player, string option)
        {
            string playerUserId = player.UserId;
            if (VotingAPI.CurrentVoting == null) return Callvote.Instance.Translation.NoVotingInProgress;
            if (!VotingAPI.CurrentVoting.Options.ContainsKey(option)) return Callvote.Instance.Translation.NoOptionAvailable.Replace("%Option%", option);
            if (VotingAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId))
            {
                if (VotingAPI.CurrentVoting.PlayerVote[playerUserId] == option) return Callvote.Instance.Translation.AlreadyVoted;
                VotingAPI.CurrentVoting.Counter[VotingAPI.CurrentVoting.PlayerVote[playerUserId]]--;
                VotingAPI.CurrentVoting.PlayerVote[playerUserId] = option;
            }

            if (!VotingAPI.CurrentVoting.PlayerVote.ContainsKey(playerUserId)) VotingAPI.CurrentVoting.PlayerVote.Add(playerUserId, option);

            VotingAPI.CurrentVoting.Counter[option]++;

            return Callvote.Instance.Translation.VoteAccepted.Replace("%Reason%", VotingAPI.CurrentVoting.Options[option]);
        }
        public static IEnumerator<float> StartVotingCoroutine(Voting newVote)
        {
            int timerCounter = 0;
            VotingAPI.CurrentVoting = newVote;
            string firstBroadcast = Callvote.Instance.Translation.AskedQuestion.Replace("%Question%", VotingAPI.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
            {
                if (counter == 0)
                {
                    firstBroadcast += $"|  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                }
                else
                {
                    firstBroadcast += $"  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";
                }
                counter++;
            }
            int textsize = firstBroadcast.Length / 10;
            if (Callvote.Instance.Config.BroadcastSize != 0)
            {
                textsize = 52 - Callvote.Instance.Config.BroadcastSize;
            }
            Map.Broadcast(5, $"<size={52 - textsize}>{firstBroadcast}</size>");
            yield return Timing.WaitForSeconds(5f);
            while (true)
            {
                if (timerCounter >= Callvote.Instance.Config.VoteDuration + 1)
                {
                    if (VotingAPI.CurrentVoting.Callback == null)
                    {
                        string resultsBroadcast = Callvote.Instance.Translation.Results;
                        foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
                        {
                            resultsBroadcast += Callvote.Instance.Translation.OptionAndCounter
                                .Replace("%Option%", kvp.Value)
                                .Replace("%OptionKey%", kvp.Key)
                                .Replace("%Counter%", VotingAPI.CurrentVoting.Counter[kvp.Key].ToString());
                            textsize = resultsBroadcast.Length / 10;
                            if (Callvote.Instance.Config.BroadcastSize != 0)
                            {
                                textsize = 48 - Callvote.Instance.Config.BroadcastSize;
                            }
                        }
                        Map.Broadcast(5, $"<size={48 - textsize}>{resultsBroadcast}</size>");
                    }
                    else
                    {
                        VotingAPI.CurrentVoting.Callback.Invoke(VotingAPI.CurrentVoting);
                    }
                    VotingAPI.CurrentVoting.Stop();
                    yield break;
                }

                {
                    string timerBroadcast = firstBroadcast + "\n";
                    foreach (KeyValuePair<string, string> kvp in VotingAPI.CurrentVoting.Options)
                    {
                        timerBroadcast += Callvote.Instance.Translation.OptionAndCounter
                            .Replace("%Option%", kvp.Value)
                            .Replace("%OptionKey%", kvp.Key)
                            .Replace("%Counter%", VotingAPI.CurrentVoting.Counter[kvp.Key].ToString());
                        textsize = timerBroadcast.Length / 10;
                        if (Callvote.Instance.Config.BroadcastSize != 0)
                        {
                            textsize = 52 - Callvote.Instance.Config.BroadcastSize;
                        }
                    }
                    Map.Broadcast(1, $"<size={52 - textsize}>{timerBroadcast}</size>");
                }
                timerCounter++;

                yield return Timing.WaitForSeconds(1f);
            }
        }
        public static string Rig(string argument)
        {
            VotingAPI.CurrentVoting.Counter[argument]++;
            return $"Rigged LMAO {argument}";
        }

        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingbase)
        {

            if (VotingAPI.CurrentVoting == null)
                return;
            if (settingbase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                ICommand existingCommand;
                switch (keybindSetting.SettingId)
                {
                    case int id when id == 888:
                        if (QueryProcessor.DotCommandHandler.TryGetCommand("cv" + Callvote.Instance.Translation.CommandYes, out existingCommand))
                        {
                            Vote(Player.Get(sender), existingCommand.Command);
                            break;
                        }
                        Vote(Player.Get(sender), Callvote.Instance.Translation.CommandYes);
                        break;
                    case int id when id == 889:
                        if (QueryProcessor.DotCommandHandler.TryGetCommand("cv" + Callvote.Instance.Translation.CommandNo, out existingCommand))
                        {
                            Vote(Player.Get(sender), existingCommand.Command);
                            break;
                        }
                        Vote(Player.Get(sender), Callvote.Instance.Translation.CommandNo);
                        break;
                    case int id when id == 890:
                        if (QueryProcessor.DotCommandHandler.TryGetCommand("cv" + Callvote.Instance.Translation.CommandMobileTaskForce, out existingCommand))
                        {
                            Vote(Player.Get(sender), existingCommand.Command);
                            break;
                        }
                        Vote(Player.Get(sender), Callvote.Instance.Translation.CommandMobileTaskForce);
                        break;
                    case int id when id == 891:
                        if (QueryProcessor.DotCommandHandler.TryGetCommand("cv" + Callvote.Instance.Translation.CommandChaosInsurgency, out existingCommand))
                        {
                            Vote(Player.Get(sender), existingCommand.Command);
                            break;
                        }
                        Vote(Player.Get(sender), Callvote.Instance.Translation.CommandChaosInsurgency);
                        break;
                }
            }
        }
    }
}

public delegate void CallvoteFunction(Voting vote);
