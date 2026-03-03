using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="ReferenceHub"/> that is voting on a <see cref="Vote"/> with a specific <see cref="VoteOption"/>.
    /// </summary>
    public class VotingEventArgs : System.EventArgs, IReferenceHubEvent, IVoteEvent, IVoteOptionEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VotingEventArgs"/> class.
        /// </summary>
        /// <param name="voteOption"><inheritdoc cref="VoteOption"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        public VotingEventArgs(Vote vote, VoteOption voteOption, bool isAllowed = true)
        {
            this.Vote = vote;
            this.VoteOption = voteOption;
            this.IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public ReferenceHub ReferenceHub => this.Vote.CallVotePlayer;

        /// <inheritdoc />
        public Vote Vote { get; }

        /// <inheritdoc />
        public VoteOption VoteOption { get; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }
    }
}
