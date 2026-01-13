#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Wrappers;
#endif
using Callvote.Features;
using Callvote.Features.Enums;
using Callvote.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class NukeVoting : Voting, IVotingTemplate
    {
        public NukeVoting(Player player) : base(
            ReplacePlayer(player),
            nameof(VotingTypeEnum.Nuke),
            player,
            AddCallback,
            AddOptions())
        {
        }

        public static void AddCallback(Voting vote)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= Callvote.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.FoundationNuked)}>{Callvote.Instance.Translation.FoundationNuked
                    .Replace("%VotePercent%", yesVotePercent.ToString())}</size>");
                Warhead.Start();
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullNuke)}>{Callvote.Instance.Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", Callvote.Instance.Config.ThresholdNuke.ToString())}</size>");
            }
        }

        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }

        private static string ReplacePlayer(Player player)
        {
            return Callvote.Instance.Translation.AskedToNuke
                    .Replace("%Player%", player.Nickname);
        }
    }
}
