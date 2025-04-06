using Callvote.Features;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;

namespace Callvote.API
{
    public static class VotingHandler
    {
        public static Voting CurrentVoting { get; private set; }
        public static Queue<Voting> VotingQueue { get; private set; } = new Queue<Voting>();
        public static Dictionary<Player, int> PlayerCallVotingAmount { get; private set; } = new Dictionary<Player, int>();
        public static Dictionary<string, string> Options { get; private set; } = new Dictionary<string, string>();
        public static bool IsQueuePaused { get; set; } = false;
        public static string Response { get; set; } = string.Empty;

        public static void CallVoting(string question, string votingType, Player player, CallvoteFunction callback, Dictionary<string, string> options = null)
        {
            Voting voting = options == null ? new Voting(question, votingType, player, callback) : new Voting(question, votingType, player, callback, options);
            Options.Clear();

            if (Callvote.Instance.Config.EnableQueue)
            {
                if (VotingQueue.Count >= Callvote.Instance.Config.QueueSize)
                {
                    Response = Callvote.Instance.Translation.QueueIsFull;
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
            if (CurrentVoting != null)
            {
                if (CurrentVoting.Callback == null)
                {
                    DisplayMessageHelper.DisplayResultsMessage();
                }
                else
                {
                    CurrentVoting.Callback.Invoke(CurrentVoting);
                }
                DcWebhook.ResultsMessage(CurrentVoting);
            }
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
            Response = Callvote.Instance.Translation.VotingEnqueued;
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
            VotingHandler.CurrentVoting = newVote;
            int timerCounter = 0;
            DisplayMessageHelper.DisplayFirstMessage(out string firstMessage);
            yield return Timing.WaitForSeconds(5f);
            while (true)
            {
                if (timerCounter >= Callvote.Instance.Config.VoteDuration + 1)
                {
                    FinishVoting();
                    yield break;
                }
                DisplayMessageHelper.DisplayWhileVotingMessage(firstMessage);
                timerCounter++;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        public static void Clear()
        {
            PlayerCallVotingAmount?.Clear();
            Options?.Clear();
            VotingQueue?.Clear();
            FinishVoting();
            Response = string.Empty;
            IsQueuePaused = false;
        }
    }
}

public delegate void CallvoteFunction(Voting vote);