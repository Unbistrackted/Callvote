using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Enums;
using Utils.NonAllocLINQ;

namespace Callvote.API.Features.Votes
{
    /// <summary>
    /// Represents the static handler that manages <see cref="Vote"/> lifecycle and the <see cref="Vote"/> queue.
    /// Provides methods to call, finish, enqueue, start <see cref="Vote"/>, add options, and many more.
    /// </summary>
    public static class VoteHandler
    {
#if !BAREBONES
        static VoteHandler()
        {
            if (UnityEngine.Application.productName == "SCPSL")
            {
                LabApi.Events.Handlers.ServerEvents.RoundRestarted += () => FinishVote(true);
                LabApi.Events.Handlers.ServerEvents.WaitingForPlayers += () => FinishVote(true);
            }
        }
#endif

        public static IReadOnlyCollection<Vote> ReadOnlyActiveParallelVotes => ActiveParallelVotes;

        internal static HashSet<Vote> ActiveParallelVotes { get; } = new HashSet<Vote>();

        /// <summary>
        /// Gets the currently active <see cref="Vote"/> instance. Null when no vote is in progress.
        /// </summary>
        public static Vote CurrentVote { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Vote"/> is currently active.
        /// </summary>
        public static bool IsVoteActive => CurrentVote != null;

        /// <summary>
        /// Gets or sets a value indicating whether the Discord Webhook will be able to send a webhook message.
        /// </summary>
        public static bool ShouldSendWebhookMessage { get; set; } = false;

        /// <summary>
        /// Request to start a <see cref="Vote"/>.
        /// If queueing is enabled the <paramref name="vote"/> will be enqueued and the <see cref="Vote"/> is started immediately.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> to start or enqueue.</param>
        /// <param name="isParallel">If the vote will be ran in parallel.</param>
        /// <returns>A <see cref="CallVoteStatus"/> representing if the action was sucessfull, or for example, if the queue is full.</returns>
        public static CallVoteStatus CallVote(Vote vote, bool isParallel = false)
        {
            if (vote == null)
            {
                throw new ArgumentNullException(nameof(vote), "Vote cannot be null!");
            }

            if (isParallel)
            {
                ActiveParallelVotes.Add(vote);
                return CurrentVote.StartVote();
            }

            if (!IsVoteActive)
            {
                CurrentVote = vote;
                return CurrentVote.StartVote();
            }

            return CallVoteStatus.VoteInProgress;
        }

        /// <summary>
        /// Finishes and clears the active <see cref="Vote"/>.
        /// Stops the vote, displays results (or invokes a callback when provided).
        /// </summary>
        /// <param name="isForced">If the voting will display the results message or invoke the Callback.</param>
        public static void FinishVote(bool isForced = false)
        {
            if (!IsVoteActive)
            {
                return;
            }

            CurrentVote?.FinishVote(isForced);

            CurrentVote = null;
        }

        public static void StopParallelVote(Vote vote, bool isForced = false)
        {
            if (vote == null)
            {
                return;
            }

            if (!ActiveParallelVotes.Contains(vote) && !vote.IsCoroutineActive)
            {
                return;
            }

            CurrentVote?.FinishVote(isForced);
            ActiveParallelVotes.Remove(vote);
        }

        public static void StopParallelVote(long voteId, bool isForced = false)
        {
            if (voteId == 0)
            {
                return;
            }

            if (!ActiveParallelVotes.TryGetFirst(v => v.VoteId == voteId, out Vote vote))
            {
                return;
            }

            StopParallelVote(vote, isForced);
        }

        public static void StopParallelVotes(UserIndentifier user, bool isForced = false)
        {
            if (user == null)
            {
                return;
            }

            foreach (Vote vote in ActiveParallelVotes.Where(v => v.CallVotePlayer == user))
            {
                StopParallelVote(vote, isForced);
            }
        }

        public static void StopParallelVotes(IEnumerable<Vote> votes, bool isForced = false)
        {
            if (votes == null)
            {
                return;
            }

            foreach (Vote vote in votes)
            {
                StopParallelVote(vote, isForced);
            }
        }

        public static void StopAllParallelVotes(bool isForced = false)
        {
            foreach (Vote vote in ActiveParallelVotes)
            {
                StopParallelVote(vote, isForced);
            }

            ActiveParallelVotes.Clear();
        }

        /// <summary>
        /// Creates a <see cref="VoteOption"/>.
        /// </summary>
        /// <param name="option">The <see cref="VoteOption"/> Option.</param>
        /// <param name="detail">The <see cref="VoteOption"/> Detail.</param>
        /// <returns>A new a <see cref="VoteOption"/>.</returns>
        public static VoteOption CreateVoteOption(string option, string detail)
        {
            return new VoteOption(option, detail);
        }
    }
}
