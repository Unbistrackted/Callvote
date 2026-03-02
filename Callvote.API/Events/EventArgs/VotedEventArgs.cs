using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="ReferenceHub"/> that has voted on a <see cref="Vote"/> with a specific <see cref="VoteOption"/>.
    /// </summary>
    public class VotedEventArgs : System.EventArgs, IReferenceHubEvent, IVoteEvent, IVoteOptionEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VotedEventArgs"/> class.
        /// </summary>
        /// <param name="voteOption"><inheritdoc cref="VoteOption"/></param>
        public VotedEventArgs(VoteOption voteOption)
        {
            this.VoteOption = voteOption;
        }

        /// <inheritdoc />
        public ReferenceHub ReferenceHub => this.Vote.CallVotePlayer;

        /// <inheritdoc />
        public Vote Vote => this.VoteOption.Vote;

        /// <inheritdoc />
        public VoteOption VoteOption { get; }
    }
}
