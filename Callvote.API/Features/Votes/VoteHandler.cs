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

        /// <summary>
        /// Gets the list of active parallel <see cref="Vote"/>s. These votes are ran in parallel with the main <see cref="CurrentVote"/> and are not affected by it.
        /// </summary>
        public static IReadOnlyCollection<Vote> ReadOnlyActiveParallelVotes => ActiveParallelVotes;

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
        /// Gets the lists of active parallel <see cref="Vote"/>s. These votes are ran in parallel with the main <see cref="CurrentVote"/> and are not affected by it.
        /// </summary>
        internal static HashSet<Vote> ActiveParallelVotes { get; } = new HashSet<Vote>();

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

        /// <summary>
        /// Stops a parallel <see cref="Vote"/>. If the <paramref name="vote"/> is not active or is not in the <see cref="ActiveParallelVotes"/> list, the method does nothing.
        /// </summary>
        /// <param name="vote">The <see cref="Vote"/> to be stopped.</param>
        /// <param name="isForced">If it's forced.</param>
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

        /// <summary>
        /// Stops a parallel <see cref="Vote"/> by its ID. If the <paramref name="voteId"/> is not active or is not in the <see cref="ActiveParallelVotes"/> list, the method does nothing.
        /// </summary>
        /// <param name="voteId">The <see cref="Vote.VoteId"/> to stop the <see cref="Vote"/>.</param>
        /// <param name="isForced">If it's forced.</param>
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

        /// <summary>
        /// Stops all active parallel <see cref="Vote"/>s associated with the specified user.
        /// </summary>
        /// <param name="user">The identifier of the user whose parallel votes are to be stopped. Cannot be null.</param>
        /// <param name="isForced">If it's forced.</param>
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

        /// <summary>
        /// Stops a collection of parallel <see cref="Vote"/>s.
        /// </summary>
        /// <param name="votes">The collection of parallel <see cref="Vote"/>s.</param>
        /// <param name="isForced">If it's forced.</param>
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

        /// <summary>
        /// Stops all active parallel votes and clears the list of active votes.
        /// </summary>
        /// <param name="isForced">If it's forced.</param>
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
