#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features;
using Callvote.Features.Interfaces;

namespace Callvote.API.VoteTemplate
{
    /// <summary>
    /// Represents the type that creates a <see cref="CustomVote"/>.
    /// </summary>
    public class CustomVote : Vote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVote"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that called the vote.</param>
        /// <param name="question">A <see cref="string"/> that represents the vote question.</param>
        /// <param name="voteType">A <see cref="string"/> that represents the vote type.</param>
        /// <param name="callback">A <see cref="Action{T}"/> that takes in a <see cref="Vote"/> that works as a callback.</param>
        /// <param name="options">A <see cref="HashSet{Vote}"/> that takes in a <see cref="VoteOption"/>. If null, uses <see cref="VoteHandler.TemporaryVoteOptions"/> instead.</param>
        /// <param name="players">A <see cref="IEnumerable{Player}"/> that takes <see cref="Player"/>s that are only allowed to see and vote in a <see cref="Vote"/>. If null, gets all ready players instead.</param>
        public CustomVote(Player player, string question, string voteType, Action<Vote> callback = null, HashSet<VoteOption> options = null, IEnumerable<Player> players = null)
            : base(player, question, voteType, callback, options, players)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVote"/> class with the <see cref="Vote.VoteOptions"/> and <see cref="Vote.Callback"/> from a <see cref="IPredefinedVote"/>.
        /// </summary>
        /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Vote.Question"/>.</param>
        /// <param name="voteType"><see cref="Vote.VoteType"/>.</param>
        /// <param name="voteTemplate">The <see cref="IPredefinedVote"/> to be copied from.</param>
        public CustomVote(Player player, string question, string voteType, IPredefinedVote voteTemplate)
            : base(player, question, voteType, voteTemplate.Callback, voteTemplate.VoteOptions)
        {
        }
    }
}
