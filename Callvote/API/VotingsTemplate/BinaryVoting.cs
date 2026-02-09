#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features;

namespace Callvote.API.VotingsTemplate
{
    /// <summary>
    /// Represents the type for the <see cref="BinaryVoting"/>, which only contains the Yes and No options from the Translation File.
    /// </summary>
    public class BinaryVoting : Voting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryVoting"/> class.
        /// </summary>
        /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Voting.Question"/>.</param>
        /// <param name="votingType"><see cref="Voting.VotingType"/>.</param>
        /// <param name="callback"><see cref="Voting.Callback"/>.</param>
        /// <param name="players"><see cref="Voting.AllowedPlayers"/>.</param>
        public BinaryVoting(Player player, string question, string votingType, Action<Voting> callback = null, IEnumerable<Player> players = null)
            : base(player, question, votingType, callback, AddVotes(out Vote yes, out Vote no), players)
        {
            this.YesVote = yes;
            this.NoVote = no;
        }

        /// <summary>
        /// Gets the Yes <see cref="Vote"/> option.
        /// </summary>
        public Vote YesVote { get; }

        /// <summary>
        /// Gets the No <see cref="Vote"/> option.
        /// </summary>
        public Vote NoVote { get; }

        private static HashSet<Vote> AddVotes(out Vote yes, out Vote no)
        {
            yes = new Vote(CallvotePlugin.Instance.Translation.CommandYes, CallvotePlugin.Instance.Translation.OptionYes);
            no = new Vote(CallvotePlugin.Instance.Translation.CommandNo, CallvotePlugin.Instance.Translation.OptionNo);

            return [yes, no];
        }
    }
}
