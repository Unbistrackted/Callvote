using Callvote.API.Events.Interfaces;
using Callvote.API.Features.Votes;

namespace Callvote.API.Events.EventArgs
{
    /// <summary>
    /// Contains all information about a <see cref="Vote"/> that is ending.
    /// </summary>
    public class VoteEndingEventArgs : System.EventArgs, IVoteEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteEndingEventArgs"/> class.
        /// </summary>
        /// <param name="vote"><inheritdoc cref="Vote"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public VoteEndingEventArgs(Vote vote, bool isAllowed = true)
        {
            this.Vote = vote;
            this.IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public Vote Vote { get; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Gets the vote option that has received the most votes.
        /// </summary>
        public VoteOption WinningVoteOption => this.Vote.GetWinningVoteOption();
    }
}
