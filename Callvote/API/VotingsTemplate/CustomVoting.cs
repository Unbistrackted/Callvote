#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features;
using Callvote.Features.Interfaces;

namespace Callvote.API.VotingsTemplate
{
    /// <summary>
    /// Represents the type that creates a <see cref="CustomVoting"/>.
    /// </summary>
    public class CustomVoting : Voting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVoting"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that called the voting.</param>
        /// <param name="question">A <see cref="string"/> that represents the voting question.</param>
        /// <param name="votingType">A <see cref="string"/> that represents the voting type.</param>
        /// <param name="callback">A <see cref="Action{T}"/> that takes in a <see cref="Voting"/> that works as a callback.</param>
        /// <param name="options">A <see cref="HashSet{Vote}"/> that takes in a <see cref="Vote"/>. If null, uses <see cref="VotingHandler.TemporaryVoteOptions"/> instead.</param>
        /// <param name="players">A <see cref="IEnumerable{Player}"/> that takes <see cref="Player"/>s that are only allowed to see and vote in a <see cref="Voting"/>. If null, gets all ready players instead.</param>
        public CustomVoting(Player player, string question, string votingType, Action<Voting> callback = null, HashSet<Vote> options = null, IEnumerable<Player> players = null)
            : base(player, question, votingType, callback, options, players)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVoting"/> class with the <see cref="Voting.VoteOptions"/> and <see cref="Voting.Callback"/> from a <see cref="IVotingTemplate"/>.
        /// </summary>
        /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Voting.Question"/>.</param>
        /// <param name="votingType"><see cref="Voting.VotingType"/>.</param>
        /// <param name="votingTemplate">The <see cref="IVotingTemplate"/> to be copied from.</param>
        public CustomVoting(Player player, string question, string votingType, IVotingTemplate votingTemplate)
            : base(player, question, votingType, votingTemplate.Callback, votingTemplate.VoteOptions)
        {
        }
    }
}