using Callvote.Features;
using Exiled.API.Features;

namespace Callvote.API.VotingsTemplate
{
    public class BinaryVoting : Voting
    {
        public BinaryVoting(Player player, string question, string votingType, CallvoteFunction callback) : base(question, votingType, player, callback, VotingHandler.Options)
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
        }
    }
}
