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
        /// <inheritdoc/>
        public CustomVoting(Player player, string question, string votingType, Action<Voting> callback = null, Dictionary<string, string> options = null, IEnumerable<Player> players = null)
            : base(player, question, votingType, callback, options, players)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVoting"/> class with the <see cref="Voting.Options"/> and <see cref="Voting.Callback"/> from a <see cref="IVotingTemplate"/>.
        /// </summary>
        /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
        /// <param name="question"><see cref="Voting.Question"/>.</param>
        /// <param name="votingType"><see cref="Voting.VotingType"/>.</param>
        /// <param name="votingTemplate">The <see cref="IVotingTemplate"/> to be copied from.</param>
        public CustomVoting(Player player, string question, string votingType, IVotingTemplate votingTemplate)
            : base(player, question, votingType, votingTemplate.Callback, votingTemplate.Options)
        {
        }
    }
}