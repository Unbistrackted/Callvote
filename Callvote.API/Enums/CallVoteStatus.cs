using Callvote.API.Features.Votes;

namespace Callvote.API.Enums
{
    /// <summary>
    /// Represents the enumeration for the <see cref="VoteHandler.CallVote(Vote, bool)"/> Status.
    /// </summary>
    public enum CallVoteStatus
    {
        /// <summary>The <see cref="Vote"/> was cancelled.</summary>
        VoteCancelled,

        /// <summary><see cref="Vote"/>  was enqueued.</summary>
        VoteEnqueued = -1,

        /// <summary><see cref="Vote"/>  has started.</summary>
        VoteStarted,

        /// <summary>There is a <see cref="Vote"/> currently in progress.</summary>
        VoteInProgress,

        /// <summary>The Queue is full.</summary>
        QueueIsFull,

        /// <summary>The Queue is disabled.</summary>
        QueueDisabled,

        /// <summary>The Player reached the maximum ammount of votes in a round.</summary>
        MaxedCallVotes,
    }
}
