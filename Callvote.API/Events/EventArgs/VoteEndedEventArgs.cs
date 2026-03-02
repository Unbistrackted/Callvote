using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="Vote"/> that has ended.
    /// </summary>
    public class VoteEndedEventArgs : System.EventArgs, IVoteEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteEndedEventArgs"/> class.
        /// </summary>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        public VoteEndedEventArgs(Vote vote)
        {
            this.Vote = vote;
        }

        /// <inheritdoc />
        public Vote Vote { get; }

        /// <summary>
        /// Gets the vote option that has received the most votes.
        /// </summary>
        public VoteOption WinningVoteOption => this.Vote.GetWinningVoteOption();
    }
}
