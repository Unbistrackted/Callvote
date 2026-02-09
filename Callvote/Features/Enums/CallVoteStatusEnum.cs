using Callvote.API;

namespace Callvote.Features.Enums
{
    /// <summary>
    /// Represents the enumeration for the <see cref="VoteHandler.CallVote(Vote)"/> Status.
    /// </summary>
    public enum CallVoteStatusEnum
    {
#pragma warning disable SA1602 // Enumeration items should be documented - The enum values are self-explanatory.
        VoteEnqueued = -1,
        VoteStarted,
        VoteInProgress,
        QueueIsFull,
        QueueDisabled,
        MaxedCallVotes,
#pragma warning restore SA1602 // Enumeration items should be documented - The enum values are self-explanatory.
    }
}
