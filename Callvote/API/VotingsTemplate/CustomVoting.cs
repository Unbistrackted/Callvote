using Callvote.Features;
using Callvote.Interface;
using Exiled.API.Features;
using System.Collections.Generic;

namespace Callvote.API.VotingsTemplate
{
    public class CustomVoting : Voting
    {
        public CustomVoting(Player player, string question, string votingType, CallvoteFunction callback, Dictionary<string, string> options = null) : base(question, votingType, player, callback, options ?? VotingHandler.Options)
        {
        }

        public CustomVoting(Player player, string question, string votingType, IVotingTemplate votingTemplate) : base(question, votingType, player, votingTemplate.Callback, votingTemplate.Options)
        {
        }
    }
}