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
    public class NukeVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Nuke), AddCallback), IVotingTemplate
    {
        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= Callvote.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.FoundationNuked)}>{Callvote.Instance.Translation.FoundationNuked
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
                Warhead.Start();
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullNuke)}>{Callvote.Instance.Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", Callvote.Instance.Config.ThresholdNuke.ToString())}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToNuke.Replace("%Player%", player.Nickname);
        }
    }
}
