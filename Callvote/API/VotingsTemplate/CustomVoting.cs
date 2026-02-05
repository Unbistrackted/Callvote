#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features;
using System.Collections.Generic;
using Callvote.Features.Interfaces;
using System;

namespace Callvote.API.VotingsTemplate
{
    public class CustomVoting : Voting
    {
        public CustomVoting(Player player, string question, string votingType, Action<Voting> callback = null, Dictionary<string, string> options = null, IEnumerable<Player> players = null) : base(player, question, votingType, callback, options ?? VotingHandler.Options, players)
        {
        }

        public CustomVoting(Player player, string question, string votingType, IVotingTemplate votingTemplate) : base(player, question, votingType, votingTemplate.Callback, votingTemplate.Options)
        {
        }
    }
}