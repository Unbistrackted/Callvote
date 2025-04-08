using Callvote.Enums;
using Callvote.Features;
using Exiled.API.Features;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class NukeVoting : Voting
    {
        public NukeVoting(Player player) : base(
            ReplacePlayer(player),
            nameof(VotingTypeEnum.Nuke),
            player,
            vote =>
            {
                int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                if (yesVotePercent >= Callvote.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.FoundationNuked
                        .Replace("%VotePercent%", yesVotePercent.ToString()));
                    Warhead.Start();
                }
                else
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullNuke
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%ThresholdNuke%", Callvote.Instance.Config.ThresholdNuke.ToString()));
                }
            },
            VotingHandler.Options)
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToNuke
                    .Replace("%Player%", player.Nickname);
        }
    }
}
