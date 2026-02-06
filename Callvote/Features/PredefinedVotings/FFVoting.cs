#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using System;
using System.Linq;
using Callvote.API;
using Callvote.API.VotingsTemplate;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;

namespace Callvote.Features.PredefinedVotings
{
    /// <summary>
    /// Represents the type for the Friendly Fire enable/disable Predefined Voting.
    /// Initializes a new instance of the <see cref="FFVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class FFVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Ff), AddCallback), IVotingTemplate
    {
        private static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= CallvotePlugin.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
            {
                Server.FriendlyFire = !Server.FriendlyFire;
                string msg = Server.FriendlyFire
                    ? CallvotePlugin.Instance.Translation.DisablingFriendlyFire
                    : CallvotePlugin.Instance.Translation.EnablingFriendlyFire;
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg.Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
            else
            {
                string msg = Server.FriendlyFire
                    ? CallvotePlugin.Instance.Translation.NoSuccessFullEnableFf
                    : CallvotePlugin.Instance.Translation.NoSuccessFullDisableFf;
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(msg)}>{msg
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdFF%", CallvotePlugin.Instance.Config.ThresholdFf.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            string baseQuestion = Server.FriendlyFire
                ? CallvotePlugin.Instance.Translation.AskedToDisableFf
                : CallvotePlugin.Instance.Translation.AskedToEnableFf;

            return baseQuestion.Replace("%Player%", player.Nickname);
        }
    }
}
