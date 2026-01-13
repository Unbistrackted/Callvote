#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features;
using MEC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Callvote.API
{
    /// <summary>
    /// Central static handler that manages <see cref="Voting"/> lifecycle and the <see cref="Voting"/> queue.
    /// Provides methods to call, finish, enqueue and start <see cref="Voting"/>, and keeps track of options and per-player call counts.
    /// </summary>
    public static class VotingHandler
    {
        /// <summary>
        /// The currently active <see cref="Voting"/> instance. Null when no vote is in progress.
        /// </summary>
        public static Voting CurrentVoting { get; private set; }

        /// <summary>
        /// Queue of pending <see cref="Voting"/> instances. When queueing is enabled, new votes are placed here.
        /// </summary>
        public static Queue<Voting> VotingQueue { get; private set; } = new Queue<Voting>();

        /// <summary>
        /// Tracks how many <see cref="Voting"/> each player has initiated during the current round.
        /// Key: <see cref="Player"/> who called <see cref="Voting"/>. Value: number of <see cref="Voting"/> they've called.
        /// </summary>
        public static Dictionary<Player, int> PlayerCallVotingAmount { get; private set; } = new Dictionary<Player, int>();

        /// <summary>
        /// Temporary mapping for the commands and labels/options.
        /// This is cleared when <see cref="CallVoting"/> is invoked.
        /// Key: Command name. Value: Option/Label name for the command.
        /// </summary>
        public static Dictionary<string, string> Options { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// When true the vote queue will not start the next voting even if entries exist.
        /// </summary>
        public static bool IsQueuePaused { get; set; } = false;

        /// <summary>
        /// Response message set by operations on the handler (e.g. "Queue is full" or "Voting enqueued").
        /// Used for upstream code to read and display to users.
        /// </summary>
        public static string Response { get; set; } = string.Empty;

        /// <summary>
        /// Request to start a <see cref="Voting"/>. 
        /// If queueing is enabled the <paramref name="vote"/> will be enqueued
        /// and <see cref="TryStartNextVoting"/> will be attempted. If queueing is disabled and <see cref="CurrentVoting"/> is null,
        /// the <see cref="Voting"/> is started immediately.
        /// </summary>
        /// <param name="vote">The <see cref="Voting"/> to start or enqueue.</param>
        public static void CallVoting(Voting vote)
        {
            Options.Clear();

            if (Callvote.Instance.Config.EnableQueue)
            {
                if (VotingQueue.Count >= Callvote.Instance.Config.QueueSize)
                {
                    Response = Callvote.Instance.Translation.QueueIsFull;
                    return;
                }

                VotingQueue.Enqueue(vote);
                TryStartNextVoting();
                return;
            }

            if (CurrentVoting == null)
            {
                CurrentVoting = vote;
                CurrentVoting.Start();
            }
        }

        /// <summary>
        /// Finishes and clears the active <see cref="Voting"/>.
        /// Stops the vote, displays results (or invokes a callback when provided),
        /// sends results to configured Discord webhook asynchronously, clears <see cref="CurrentVoting"/>, and starts the next
        /// queued <see cref="Voting"/> if queueing is enabled.
        /// </summary>
        public static void FinishVoting()
        {
            CurrentVoting?.Stop();

            if (CurrentVoting != null)
            {
                if (CurrentVoting.Callback == null)
                    DisplayMessageHelper.DisplayResultsMessage();
                else
                    CurrentVoting.Callback.Invoke(CurrentVoting);

                _ = Task.Run(async () => await Features.DiscordWebhook.ResultsMessage(CurrentVoting));
            }

            CurrentVoting = null;

            if (Callvote.Instance.Config.EnableQueue)
                TryStartNextVoting();
        }

        /// <summary>
        /// Attempts to start the next <see cref="Voting"/> in the <see cref="VotingQueue"/>. If no <see cref="Voting"/> is in progress and the <see cref="VotingQueue"/> is not paused and contains items,
        /// dequeues the next <see cref="Voting"/> and starts it. Otherwise sets <see cref="Response"/> to indicate that <see cref="Voting"/> was enqueued.
        /// </summary>
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

        /// <summary>
        /// Adds a option for the <see cref="Voting"/> build process.
        /// Only adds the option if the <paramref name="command"/> key does not already exist.
        /// </summary>
        /// <param name="command">The internal command (e.g. console alias).</param>
        /// <param name="option">Human-readable option text/label for the command.</param>
        public static void AddOptionToVoting(string command, string option)
        {
            if (!Options.ContainsKey(command))
                Options[command] = option;
        }

        /// <summary>
        /// Coroutine that manages the <see cref="CurrentVoting"/> runtime. This method yields to MEC timing and repeatedly refreshes the voting display
        /// until the <see cref="CurrentVoting"/> duration has passed. When the courotine expires when the <see cref="CurrentVoting"/> is finished.
        /// </summary>
        /// <param name="newVote">The vote instance to run the coroutine for.</param>
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
                yield return Timing.WaitForSeconds(Callvote.Instance.Config.RefreshInterval);
            }
        }

        /// <summary>
        /// Clears handler state: per-player call counters, option mappings, queued votes, stops any running vote,
        /// clears <see cref="Response"/> and unpauses the queue.
        /// Used during round resets or plugin reloads.
        /// </summary>
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

/// <summary>
/// Delegate signature for a callback that can be invoked when a vote completes. Implementations receive the completed <see cref="Voting"/>.
/// </summary>
/// <param name="vote">The completed <see cref="Voting"/> instance.</param>
public delegate void CallvoteFunction(Voting vote);