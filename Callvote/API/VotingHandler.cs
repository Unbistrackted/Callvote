using Callvote.API.Objects;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;

namespace Callvote.API
{
    public static class VotingHandler
    {
        public static Voting CurrentVoting;
        public static Queue<Voting> VotingQueue;
        public static Dictionary<Player, int> PlayerCallVotingAmount;
        public static Dictionary<string, string> Options;
        public static bool IsQueuePaused;
        public static string Response;

        public static void CallVoting(string question, string votingType, Player player, CallvoteFunction callback, Dictionary<string, string> options = null)
        {
            Voting voting = options == null ? new Voting(question, votingType, player, callback) : new Voting(question, votingType, player, callback, options);
            Options.Clear();

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

            if (CurrentVoting != null)
            {
                return;
            }

            CurrentVoting = voting;
            CurrentVoting.Start();
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

        public static IEnumerator<float> VotingCoroutine(Voting newVote)
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


        public static void Init()
        {
            PlayerCallVotingAmount = new Dictionary<Player, int>();
            Options = new Dictionary<string, string>();
            Response = string.Empty;
            if (Callvote.Instance.Config.EnableQueue)
            {
                VotingQueue = new Queue<Voting>();
                IsQueuePaused = false;
            }
        }

        public static void Clean()
        {
            PlayerCallVotingAmount = null;
            Options = null;
            Response = null;
            VotingQueue = null;
            CurrentVoting = null;
        }
    }
}

public delegate void CallvoteFunction(Voting vote);