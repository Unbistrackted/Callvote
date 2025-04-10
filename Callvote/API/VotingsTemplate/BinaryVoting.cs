using Callvote.Features;
using Exiled.API.Features;
using System.Collections.Generic;

namespace Callvote.API.VotingsTemplate
{
    public class BinaryVoting : Voting
    {
        public BinaryVoting(Player player, string question, string votingType, CallvoteFunction callback) : base(question, votingType, player, callback, AddOptions())
        {
        }
        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }
    }
}
