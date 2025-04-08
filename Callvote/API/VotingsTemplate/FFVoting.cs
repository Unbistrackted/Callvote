using Callvote.Enums;
using Callvote.Features;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class FFVoting : Voting
    {
        public FFVoting(Player player) : base(
            WhichFFQuestion(player),
            nameof(VotingTypeEnum.Ff),
            player,
            vote =>
            {
                int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                if (yesVotePercent >= Callvote.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
                {
                    Server.FriendlyFire = !Server.FriendlyFire;
                    string msg = Server.FriendlyFire
                        ? Callvote.Instance.Translation.DisablingFriendlyFire
                        : Callvote.Instance.Translation.EnablingFriendlyFire;
                    Map.Broadcast(5, msg.Replace("%VotePercent%", yesVotePercent.ToString()));
                }
                else
                {
                    string msg = Server.FriendlyFire
                        ? Callvote.Instance.Translation.NoSuccessFullEnableFf
                        : Callvote.Instance.Translation.NoSuccessFullDisableFf;
                    Map.Broadcast(5, msg
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()));
                }
            },
            AddOptions())
        {
        }

        private static string WhichFFQuestion(Player player)
        {
            string baseQuestion = Server.FriendlyFire
                ? Callvote.Instance.Translation.AskedToDisableFf
                : Callvote.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }
    }
}
