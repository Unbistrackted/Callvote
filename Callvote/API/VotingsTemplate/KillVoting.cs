using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class KillVoting : Voting, IVotingTemplate
    {
        public KillVoting(Player player, Player ofender, string reason) : base(
            ReplacePlayer(player, ofender, reason),
            nameof(VotingTypeEnum.Kill),
            player,
            vote => AddCallback(vote, player, ofender, reason),
            AddOptions())
        {
        }

        public static void AddCallback(Voting vote, Player player, Player ofender, string reason)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
            if (yesVotePercent >= Callvote.Instance.Config.ThresholdKill && yesVotePercent > noVotePercent)
            {
                if (!ofender.HasPermissions("cv.untouchable"))
                {
                    ofender.Kill(reason);
                    MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.PlayerKilled)}>{Callvote.Instance.Translation.PlayerKilled
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%Player%", player.Nickname)
                        .Replace("%Offender%", ofender.Nickname)
                        .Replace("%Reason%", reason)}</size>");
                }
                if (ofender.HasPermissions("cv.untouchable")) ofender.SendBroadcast(Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()), (ushort)Callvote.Instance.Config.FinalResultsDuration);
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullKill)}>{Callvote.Instance.Translation.NoSuccessFullKill
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKill%", Callvote.Instance.Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname)}</size>");
            }
        }
        public static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }

        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return Callvote.Instance.Translation.AskedToKill
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }
    }
}
