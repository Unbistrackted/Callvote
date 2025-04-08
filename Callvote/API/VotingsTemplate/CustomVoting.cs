using Callvote.Features;
using Exiled.API.Features;
using System.Collections.Generic;

namespace Callvote.API.VotingsTemplate
{
    public class CustomVoting : Voting
    {
        public CustomVoting(Player player, string question, string votingType, CallvoteFunction callback, Dictionary<string, string> options) : base(question, votingType, player, callback, options)
        {
        }
    }
}
