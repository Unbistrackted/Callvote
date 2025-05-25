using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.API.VotingsTemplate
{
    public class KickVoting : Voting, IVotingTemplate
    {
        public KickVoting(Player player, Player ofender, string reason) : base(
            ReplacePlayer(player, ofender, reason),
            nameof(VotingTypeEnum.Kick),
            player,
            vote =>
            {
                int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                if (yesVotePercent >= Callvote.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
                {
                    if (!ofender.CheckPermission("cv.untouchable"))
                    {
                        ofender.Kick(reason);
                        MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.PlayerKicked)}>{Callvote.Instance.Translation.PlayerKicked
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%Player%", player.Nickname)
                            .Replace("%Offender%", ofender.Nickname)
                            .Replace("%Reason%", reason)}</size>");
                    }
                    if (ofender.CheckPermission("cv.untouchable")) ofender.Broadcast((ushort)Callvote.Instance.Config.FinalResultsDuration, Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
                }
                else
                {
                    MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NotSuccessFullKick)}>{Callvote.Instance.Translation.NotSuccessFullKick
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%ThresholdKick%", Callvote.Instance.Config.ThresholdKick.ToString())
                        .Replace("%Offender%", ofender.Nickname)}</size>");
                }
            },
            AddOptions())
        {
        }
        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return Callvote.Instance.Translation.AskedToKick
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }

        private static Dictionary<string, string> AddOptions()
        {
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);
            return VotingHandler.Options;
        }
    }
}
