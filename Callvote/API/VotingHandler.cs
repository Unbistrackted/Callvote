using Callvote.API;
using Callvote.VoteHandlers;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using System;
using System.Collections.Generic;
using UserSettings.ServerSpecific;

namespace Callvote.VoteHandlers
{
    public static class VotingHandler
    {
        public static Voting CurrentVoting;
        public static Queue<Voting> VotingQueue = new Queue<Voting>();
        public static Dictionary<Player, int> PlayerCallVotingAmount = new Dictionary<Player, int>();
        public static Dictionary<string, string> Options = new Dictionary<string, string>();
        public static bool IsQueuePaused = false;
        public static string Response = string.Empty;

        public static void CallVoting(string question, string votingType, Player player, CallvoteFunction callback, Dictionary<string, string> options = null)
        {
            Voting voting = options == null ? new Voting(question, votingType, player, callback) : new Voting(question, votingType, options, player, callback);
            VotingHandler.Options.Clear();

            if (Callvote.Instance.Config.EnableQueue)
            {
                if (VotingQueue.Count >= Callvote.Instance.Config.QueueSize)
                {
                    Response = "<color=red>Queue is full.</color>";
                    return;

                }
                VotingQueue.Enqueue(voting);
                TryStartNextVoting();
                return;
            }

            if (CurrentVoting == null)
            {
                CurrentVoting = voting;
                CurrentVoting.Start();
            }
        }

        public static void FinishVoting()
        {
            CurrentVoting?.Stop();
            CurrentVoting = null;
            if (Callvote.Instance.Config.EnableQueue) { TryStartNextVoting(); };
        }

        public static void TryStartNextVoting()
        {
            if (CurrentVoting == null && VotingQueue.Count != 0 && !IsQueuePaused)
            {
                CurrentVoting = VotingQueue.Dequeue();
                CurrentVoting.Start();
                return;
            }
            Response = "<color=#EDF193>Voting Enqueued.</color>";
        }

        public static void AddOptionToVoting(string command, string option)
        {
            if (!Options.ContainsKey(command))
            {
                Options[command] = option;
            }
        }

        public static IEnumerator<float> StartVotingCoroutine(Voting newVote)
        {
            int timerCounter = 0;
            VotingHandler.CurrentVoting = newVote;
            string firstBroadcast = Callvote.Instance.Translation.AskedQuestion.Replace("%Question%", VotingHandler.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
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
            HintProvider.Provider.ShowString(TimeSpan.FromSeconds(5), $"<size={52 - textsize}>{firstBroadcast}</size>");
            yield return Timing.WaitForSeconds(5f);
            while (true)
            {
                if (timerCounter >= Callvote.Instance.Config.VoteDuration + 1)
                {
                    if (VotingHandler.CurrentVoting.Callback == null)
                    {
                        string resultsBroadcast = Callvote.Instance.Translation.Results;
                        foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
                        {
                            resultsBroadcast += Callvote.Instance.Translation.OptionAndCounter
                                .Replace("%Option%", kvp.Value)
                                .Replace("%OptionKey%", kvp.Key)
                                .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
                            textsize = resultsBroadcast.Length / 10;
                            if (Callvote.Instance.Config.BroadcastSize != 0)
                            {
                                textsize = 48 - Callvote.Instance.Config.BroadcastSize;
                            }
                        }

                        HintProvider.Provider.ShowString(TimeSpan.FromSeconds(5), $"<size={48 - textsize}>{resultsBroadcast}</size>");
                    }
                    else
                    {
                        VotingHandler.CurrentVoting.Callback.Invoke(VotingHandler.CurrentVoting);
                    }
                    FinishVoting();
                    yield break;
                }

                {
                    string timerBroadcast = firstBroadcast + "\n";
                    foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
                    {
                        timerBroadcast += Callvote.Instance.Translation.OptionAndCounter
                            .Replace("%Option%", kvp.Value)
                            .Replace("%OptionKey%", kvp.Key)
                            .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
                        textsize = timerBroadcast.Length / 10;
                        if (Callvote.Instance.Config.BroadcastSize != 0)
                        {
                            textsize = 52 - Callvote.Instance.Config.BroadcastSize;
                        }
                    }
                    HintProvider.Provider.ShowString(TimeSpan.FromSeconds(1), $"<size={52 - textsize}>{timerBroadcast}</size>");
                }
                timerCounter++;
                yield return Timing.WaitForSeconds(1f);
            }
        }
        internal static void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase settingBase)
        {

            if (VotingHandler.CurrentVoting == null)
                return;
            if (settingBase is SSKeybindSetting keybindSetting && keybindSetting.SyncIsPressed)
            {
                switch (keybindSetting.SettingId)
                {
                    case int id when id == 888:
                        CurrentVoting.Vote(Player.Get(sender), CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionYes).Command);
                        break;
                    case int id when id == 889:
                        CurrentVoting.Vote(Player.Get(sender), CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionNo).Command);
                        break;
                    case int id when id == 890:
                        CurrentVoting.Vote(Player.Get(sender), CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionMtf).Command);
                        break;
                    case int id when id == 891:
                        CurrentVoting.Vote(Player.Get(sender), CurrentVoting.CommandList.GetValueSafe(Callvote.Instance.Translation.OptionCi).Command);
                        break;
                }
            }
        }
    }
}

public delegate void CallvoteFunction(Voting vote);