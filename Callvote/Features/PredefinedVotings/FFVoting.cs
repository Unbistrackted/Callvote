#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features.Enums;
using System;
using System.Linq;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Interfaces;
using Callvote.API;

namespace Callvote.Features.PredefinedVotings
{
    public class FFVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Ff), AddCallback), IVotingTemplate
    {
        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= Callvote.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                Server.FriendlyFire = !Server.FriendlyFire;
                string msg = Server.FriendlyFire
                    ? Callvote.Instance.Translation.DisablingFriendlyFire
                    : Callvote.Instance.Translation.EnablingFriendlyFire;
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg.Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
            else
            {
                string msg = Server.FriendlyFire
                    ? Callvote.Instance.Translation.NoSuccessFullEnableFf
                    : Callvote.Instance.Translation.NoSuccessFullDisableFf;
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", Callvote.Instance.Config.ThresholdFf.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire
                ? Callvote.Instance.Translation.AskedToDisableFf
                : Callvote.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
