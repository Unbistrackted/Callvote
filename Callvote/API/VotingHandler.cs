#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
#endif
using Callvote.Features;
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
        public static Voting CurrentVoting { get; internal set; }

        /// <summary>
        /// Queue of pending <see cref="Voting"/> instances. When queueing is enabled, new votes are placed here.
        /// </summary>
        public static Queue<Voting> VotingQueue { get; private set; } = new Queue<Voting>();

        /// <summary>
        /// Tracks how many <see cref="Voting"/> each player has initiated during the current round.
        /// Key: <see cref="Player"/> who called <see cref="Voting"/>. Value: number of <see cref="Voting"/> they've called.
        /// </summary>
        public static Dictionary<Player, int> PlayerCallVotingAmount { get; private set; } = [];

        /// <summary>
        /// Temporary mapping for the commands and labels/options.
        /// This is cleared when <see cref="CallVoting"/> is invoked.
        /// Key: Command name. Value: Option/Label name for the command.
        /// </summary>
        public static Dictionary<string, string> Options { get; private set; } = [];

        /// <summary>
        /// Gets whether the <see cref="Voting"/> is currently active.
        /// </summary>
        public static bool IsVotingActive => CurrentVoting != null;

        /// <summary>
        /// Gets if the Queue is full.
        /// </summary>
        public static bool IsQueueFull => VotingQueue.Count >= Callvote.Instance.Config.QueueSize;

        /// <summary>
        /// When true the vote queue will not start the next voting even if entries exist.
        /// </summary>
        public static bool IsQueuePaused { get; set; } = false;

        /// <summary>
        /// Response message set by operations on the handler (e.g. "Queue is full" or "Voting enqueued").
        /// Used for upstream code to read and display to users.
        /// </summary>
        public static string Response { get; internal set; } = string.Empty;

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
                if (IsQueueFull)
                {
                    Response = Callvote.Instance.Translation.QueueIsFull;
                    return;
                }

                VotingQueue.Enqueue(vote);
                TryStartNextVoting();
                return;
            }

            if (!IsVotingActive)
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

            if (IsVotingActive)
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
            if (!IsVotingActive && VotingQueue.Count != 0 && !IsQueuePaused)
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
        /// Checks if a player is able to call vote based on permissions and per-player call limits.
        /// </summary>
        /// <param name="player">Player to check if he is able to call a vote.</param>
        public static bool IsCallVotingAllowed(Player player)
        {
            if (IsVotingActive && !Callvote.Instance.Config.EnableQueue)
            {
                Response = Callvote.Instance.Translation.VotingInProgress;
                return false;
            }

            if (!PlayerCallVotingAmount.ContainsKey(player))
                PlayerCallVotingAmount.Add(player, 0);

            PlayerCallVotingAmount[player]++;

            if (PlayerCallVotingAmount[player] > Callvote.Instance.Config.MaxAmountOfVotesPerRound &&
#if EXILED
                !player.CheckPermission("cv.bypass"))
#else
                !player.HasPermissions("cv.bypass"))
#endif
            {
                Response = Callvote.Instance.Translation.MaxVote;
                return false;
            }

            return true;
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