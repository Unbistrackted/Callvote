#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Callvote.Configuration;
using Callvote.Features;
using Callvote.Features.Enums;

namespace Callvote.API
{
    /// <summary>
    /// Represents the static handler that manages <see cref="Vote"/> lifecycle and the <see cref="Vote"/> queue.
    /// Provides methods to call, finish, enqueue, start <see cref="Vote"/>, add options, and many more.
    /// </summary>
    public static class VoteHandler
    {
        /// <summary>
        /// Gets the currently active <see cref="Vote"/> instance. Null when no vote is in progress.
        /// </summary>
        public static Vote CurrentVote { get; internal set; }

        /// <summary>
        /// Gets queue of pending <see cref="Vote"/> instances. When queueing is enabled, new votes are placed here.
        /// </summary>
        public static Queue<Vote> VoteQueue { get; private set; } = new Queue<Vote>();

        /// <summary>
        /// Gets tracks how many <see cref="Vote"/> each player has initiated during the current round.
        /// Key: <see cref="Player"/> who called <see cref="Vote"/>. Value: number of <see cref="Vote"/> they've called.
        /// </summary>
        public static Dictionary<Player, int> PlayerCallVoteAmount { get; private set; } = [];

        /// <summary>
        /// Gets temporary mapping for the <see cref="VoteOption"/>s.
        /// This is cleared when <see cref="CallVote"/> is invoked.
        /// Cleared when the Vote starts.
        /// </summary>
        public static HashSet<VoteOption> TemporaryVoteOptions { get; private set; } = [];

        /// <summary>
        /// Gets a value indicating whether the <see cref="Vote"/> is currently active.
        /// </summary>
        public static bool IsVoteActive => CurrentVote != null;

        /// <summary>
        /// Gets a value indicating whether the Queue is full.
        /// </summary>
        public static bool IsQueueFull => VoteQueue.Count >= Config.QueueSize;

        /// <summary>
        /// Gets or sets a value indicating whether the vote queue will or will not start the next vote even if entries exist.
        /// </summary>
        public static bool IsQueuePaused { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Discord Webhook will be able to send a webhook message.
        /// </summary>
        public static bool ShouldSendWebhookMessage { get; set; } = true;

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <summary>
        /// Request to start a <see cref="Vote"/>.
        /// If queueing is enabled the <paramref name="vote"/> will be enqueued
        /// and <see cref="DequeueVote"/> will be called. If queueing is disabled and <see cref="CurrentVote"/> is null,
        /// the <see cref="Vote"/> is started immediately.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> to start or enqueue.</param>
        /// <returns>A <see cref="CallVoteStatusEnum"/> representing if the action was sucessfull, or for example, if the queue is full.</returns>
        public static CallVoteStatusEnum CallVote(Vote vote)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote), "Vote cannot be null!");
            }

            TemporaryVoteOptions.Clear();

            if (!IsCallVoteAllowed(vote.CallVotePlayer))
            {
                return CallVoteStatusEnum.MaxedCallVotes;
            }

            if (Config.EnableQueue)
            {
                if (IsQueueFull)
                {
                    return CallVoteStatusEnum.QueueIsFull;
                }

                if (IsQueuePaused)
                {
                    return CallVoteStatusEnum.QueueDisabled;
                }

                VoteQueue.Enqueue(vote);
                return DequeueVote();
            }

            if (!IsVoteActive)
            {
                CurrentVote = vote;
                CurrentVote.Start();
                return CallVoteStatusEnum.VoteStarted;
            }

            return CallVoteStatusEnum.VoteInProgress;
        }

        /// <summary>
        /// Attempts to start the next <see cref="Vote"/> in the <see cref="VoteQueue"/>. If no <see cref="Vote"/> is in progress and the <see cref="VoteQueue"/> is not paused and contains items,
        /// dequeues the next <see cref="Vote"/> and starts it.
        /// </summary>
        /// <returns>A <see cref="CallVoteStatusEnum"/> representing if the <see cref="CallVoteStatusEnum.VoteStarted"/>, or <see cref="CallVoteStatusEnum.VoteEnqueued"/> .</returns>
        public static CallVoteStatusEnum DequeueVote()
        {
            if (!IsVoteActive && VoteQueue.Count != 0 && !IsQueuePaused)
            {
                CurrentVote = VoteQueue.Dequeue();
                CurrentVote.Start();
                return CallVoteStatusEnum.VoteStarted;
            }

            return CallVoteStatusEnum.VoteEnqueued;
        }

        /// <summary>
        /// Finishes and clears the active <see cref="Vote"/>.
        /// Stops the vote, displays results (or invokes a callback when provided),
        /// sends results to configured Discord webhook asynchronously if <see cref="ShouldSendWebhookMessage"/> is true, clears <see cref="CurrentVote"/> and, starts the next
        /// queued <see cref="Vote"/> if queueing is enabled.
        /// </summary>
        public static void FinishVote()
        {
            CurrentVote?.Stop();

            if (IsVoteActive)
            {
                if (CurrentVote.Callback == null)
                {
                    DisplayMessageHelper.DisplayResultsMessage();
                }
                else
                {
                    CurrentVote.Callback.Invoke(CurrentVote);
                }

                if (ShouldSendWebhookMessage)
                {
                    _ = Task.Run(async () => await Features.DiscordWebhook.ResultsMessage(CurrentVote));
                }
            }

            CurrentVote = null;

            if (Config.EnableQueue)
            {
                DequeueVote();
            }
        }

        /// <summary>
        /// Creates a <see cref="VoteOption"/> for the <see cref="Vote"/> creation process in <see cref="TemporaryVoteOptions"/>.
        /// </summary>
        /// <param name="option">The <see cref="VoteOption"/> Option.</param>
        /// <param name="detail">The <see cref="VoteOption"/> Detail.</param>
        /// <param name="vote">The <see cref="VoteOption"/> that was created.</param>
        public static void CreateVoteOption(string option, string detail, out VoteOption vote)
        {
            vote = new VoteOption(option, detail);
            TemporaryVoteOptions.Add(vote);
        }

        /// <summary>
        /// Adds a <see cref="VoteOption"/> for the <see cref="Vote"/> creation process in <see cref="TemporaryVoteOptions"/>.
        /// </summary>
        /// <param name="vote">The <see cref="VoteOption"/> that will be added.</param>
        public static void AddVoteOption(VoteOption vote)
        {
            if (vote == null)
            {
                return;
            }

            TemporaryVoteOptions.Add(vote);
        }

        /// <summary>
        /// Checks if a Player is able to call a <see cref="Vote"/> based on per-player call limits in the config.
        /// </summary>
        /// <param name="player">Player to check if he is able to call a <see cref="Vote"/> .</param>
        /// <returns>If player is able to call a <see cref="Vote"/>.</returns>
        public static bool IsCallVoteAllowed(Player player)
        {
            if (IsVoteActive && !Config.EnableQueue)
            {
                return false;
            }

            if (!PlayerCallVoteAmount.ContainsKey(player))
            {
                PlayerCallVoteAmount.Add(player, 0);
            }

            PlayerCallVoteAmount[player]++;

            if (PlayerCallVoteAmount[player] > Config.MaxAmountOfVotesPerRound &&
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
        /// Returns the status message corresponding to the specified <see cref="CallVoteStatusEnum"/>.
        /// </summary>
        /// <param name="status">The <see cref="CallVoteStatusEnum"/> for which to retrieve the message.</param>
        /// <returns>A string containing the message for the given status; returns an empty string if the status is not
        /// recognized.</returns>
        public static string GetMessageFromCallVoteStatus(CallVoteStatusEnum status)
        {
            return status switch
            {
                CallVoteStatusEnum.VoteStarted => Translation.VoteStarted,
                CallVoteStatusEnum.VoteInProgress => Translation.VoteInProgress,
                CallVoteStatusEnum.VoteEnqueued => Translation.VoteEnqueued,
                CallVoteStatusEnum.QueueIsFull => Translation.QueueIsFull,
                CallVoteStatusEnum.QueueDisabled => Translation.QueueDisabled,
                CallVoteStatusEnum.MaxedCallVotes => Translation.MaxVote,
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Clears handler state: per-player call counters, option mappings, queued votes, stops any running vote, and unpauses the queue.
        /// Used during round resets or plugin reloads.
        /// </summary>
        public static void Clear()
        {
            PlayerCallVoteAmount?.Clear();
            TemporaryVoteOptions?.Clear();
            VoteQueue?.Clear();
            FinishVote();
            IsQueuePaused = false;
        }
    }
}
