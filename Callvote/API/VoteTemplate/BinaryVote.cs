#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features;

namespace Callvote.API.VoteTemplate
{
    /// <summary>
    /// Represents the type for the <see cref="BinaryVote"/>, which only contains the Yes and No <see cref="VoteOption"/>s from the Translation File.
    /// </summary>
    public class BinaryVote : Vote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryVote"/> class.
        /// </summary>
        /// <param name="player"><see cref="Vote.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Vote.Question"/>.</param>
        /// <param name="voteType"><see cref="Vote.VoteType"/>.</param>
        /// <param name="callback"><see cref="Vote.Callback"/>.</param>
        /// <param name="players"><see cref="Vote.AllowedPlayers"/>.</param>
        public BinaryVote(Player player, string question, string voteType, Action<Vote> callback = null, IEnumerable<Player> players = null)
            : base(player, question, voteType, callback, AddVotes(out VoteOption yes, out VoteOption no), players)
        {
            this.YesVoteOption = yes;
            this.NoVoteOption = no;
        }

        /// <summary>
        /// Gets the Yes <see cref="VoteOption"/> option.
        /// </summary>
        public VoteOption YesVoteOption { get; }

        /// <summary>
        /// Gets the No <see cref="VoteOption"/> option.
        /// </summary>
        public VoteOption NoVoteOption { get; }

        private static HashSet<VoteOption> AddVotes(out VoteOption yes, out VoteOption no)
        {
            yes = new VoteOption(CallvotePlugin.Instance.Translation.CommandYes, CallvotePlugin.Instance.Translation.DetailYes);
            no = new VoteOption(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.DetailNo);

            return [yes, no];
        }
    }
}
