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
    /// Represents the type for the enable nuke predefined voting.
    /// Initializes a new instance of the <see cref="NukeVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class NukeVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Nuke), AddCallback), IVotingTemplate
    {
        private static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= CallvotePlugin.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.FoundationNuked)}>{CallvotePlugin.Instance.Translation.FoundationNuked
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                Warhead.Detonate();
            }
            else
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.NoSuccessFullNuke)}>{CallvotePlugin.Instance.Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", CallvotePlugin.Instance.Config.ThresholdNuke.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            return CallvotePlugin.Instance.Translation.AskedToNuke.Replace("%Player%", player.Nickname);
        }
    }
}
