#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System.Collections.Generic;
using System.Threading.Tasks;
using Callvote.Features;

namespace Callvote.API
{
    /// <summary>
    /// Represents the static handler that manages <see cref="Voting"/> lifecycle and the <see cref="Voting"/> queue.
    /// Provides methods to call, finish, enqueue, start <see cref="Voting"/>, add options, and many more.
    /// </summary>
    public static class VotingHandler
    {
        /// <summary>
        /// Gets the currently active <see cref="Voting"/> instance. Null when no vote is in progress.
        /// </summary>
        public static Voting CurrentVoting { get; internal set; }

        /// <summary>
        /// Gets queue of pending <see cref="Voting"/> instances. When queueing is enabled, new votes are placed here.
        /// </summary>
        public static Queue<Voting> VotingQueue { get; private set; } = new Queue<Voting>();

        /// <summary>
        /// Gets tracks how many <see cref="Voting"/> each player has initiated during the current round.
        /// Key: <see cref="Player"/> who called <see cref="Voting"/>. Value: number of <see cref="Voting"/> they've called.
        /// </summary>
        public static Dictionary<Player, int> PlayerCallVotingAmount { get; private set; } = [];

        /// <summary>
        /// Gets temporary mapping for the commands and labels/options.
        /// This is cleared when <see cref="CallVoting"/> is invoked.
        /// Key: Command name. Value: Option/Label name for the command.
        /// </summary>
        public static Dictionary<string, string> Options { get; private set; } = [];

        /// <summary>
        /// Gets a value indicating whether gets whether the <see cref="Voting"/> is currently active.
        /// </summary>
        public static bool IsVotingActive => CurrentVoting != null;

        /// <summary>
        /// Gets a value indicating whether gets if the Queue is full.
        /// </summary>
        public static bool IsQueueFull => VotingQueue.Count >= CallvotePlugin.Instance.Config.QueueSize;

        /// <summary>
        /// Gets or sets a value indicating whether the vote queue will or will not start the next voting even if entries exist.
        /// </summary>
        public static bool IsQueuePaused { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether if the Discord Webhook will be able to send the message.
        /// </summary>
        public static bool ShouldWebhookSendMessage { get; set; } = true;

        /// <summary>
        /// Request to start a <see cref="Voting"/>.
        /// If queueing is enabled the <paramref name="vote"/> will be enqueued
        /// and <see cref="DequeueVoting"/> will be called. If queueing is disabled and <see cref="CurrentVoting"/> is null,
        /// the <see cref="Voting"/> is started immediately.
        /// </summary>
        /// <param name="vote">The <see cref="Voting"/> to start or enqueue.</param>
        /// <returns>The response message set by operations on the handler (e.g. "Queue is full" or "Voting enqueued").</returns>
        public static string CallVoting(Voting vote)
        {
            Options.Clear();

            if (CallvotePlugin.Instance.Config.EnableQueue)
            {
                if (IsQueueFull)
                {
                    return CallvotePlugin.Instance.Translation.QueueIsFull;
                }

                VotingQueue.Enqueue(vote);
                return DequeueVoting();
            }

            if (!IsVotingActive)
            {
                CurrentVoting = vote;
                return CurrentVoting.Start();
            }

            return CallvotePlugin.Instance.Translation.VotingInProgress;
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
                {
                    DisplayMessageHelper.DisplayResultsMessage();
                }
                else
                {
                    CurrentVoting.Callback.Invoke(CurrentVoting);
                }

                if (ShouldWebhookSendMessage)
                {
                    _ = Task.Run(async () => await Features.DiscordWebhook.ResultsMessage(CurrentVoting));
                }
            }

            CurrentVoting = null;

            if (CallvotePlugin.Instance.Config.EnableQueue)
            {
                DequeueVoting();
            }
        }

        /// <summary>
        /// Attempts to start the next <see cref="Voting"/> in the <see cref="VotingQueue"/>. If no <see cref="Voting"/> is in progress and the <see cref="VotingQueue"/> is not paused and contains items,
        /// dequeues the next <see cref="Voting"/> and starts it.
        /// </summary>
        /// <returns>The response message set by operations on the handler (e.g. "Queue is full" or "Voting enqueued").</returns>
        public static string DequeueVoting()
        {
            if (!IsVotingActive && VotingQueue.Count != 0 && !IsQueuePaused)
            {
                CurrentVoting = VotingQueue.Dequeue();
                return CurrentVoting.Start();
            }

            if (IsQueuePaused)
            {
                return CallvotePlugin.Instance.Translation.QueueDisabled;
            }

            return CallvotePlugin.Instance.Translation.VotingEnqueued;
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
            {
                Options[command] = option;
            }
        }

        /// <summary>
        /// Checks if a player is able to call vote based on per-player call limits.
        /// </summary>
        /// <param name="player">Player to check if he is able to call a vote.</param>
        /// <returns>If player is able to call a vote.</returns>
        public static bool IsCallVotingAllowed(Player player)
        {
            if (IsVotingActive && !CallvotePlugin.Instance.Config.EnableQueue)
            {
                return false;
            }

            if (!PlayerCallVotingAmount.ContainsKey(player))
            {
                PlayerCallVotingAmount.Add(player, 0);
            }

            PlayerCallVotingAmount[player]++;

            if (PlayerCallVotingAmount[player] > CallvotePlugin.Instance.Config.MaxAmountOfVotesPerRound &&
#if EXILED
                !player.CheckPermission("cv.bypass"))
#else
                !player.HasPermissions("cv.bypass"))
#endif
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears handler state: per-player call counters, option mappings, queued votes, stops any running vote, and unpauses the queue.
        /// Used during round resets or plugin reloads.
        /// </summary>
        public static void Clear()
        {
            PlayerCallVotingAmount?.Clear();
            Options?.Clear();
            VotingQueue?.Clear();
            FinishVoting();
            IsQueuePaused = false;
        }
    }
}