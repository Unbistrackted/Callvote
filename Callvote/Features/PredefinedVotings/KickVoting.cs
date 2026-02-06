#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
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
    /// Represents the type for the Kick Player Predefined Voting.
    /// Initializes a new instance of the <see cref="KickVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    /// <param name="ofender">The <see cref="Player"/> that is going to be kicked.</param>
    /// <param name="reason">The reason for the kick.</param>
    public class KickVoting(Player player, Player ofender, string reason) : BinaryVoting(player, ReplacePlayer(player, ofender, reason), nameof(VotingTypeEnum.Kick), vote => AddCallback(vote, player, ofender, reason)), IVotingTemplate
    {
        private static void AddCallback(Voting vote, Player player, Player ofender, string reason)
        {
            int yesVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[CallvotePlugin.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= CallvotePlugin.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
            {
#if EXILED
                if (!ofender.CheckPermission("cv.untouchable"))
#else
                if (!ofender.HasPermissions("cv.untouchable"))
#endif
                {
                    ofender.Kick(reason);
                    SoftDependency.MessageProvider.DisplayMessage(
                        TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                        $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.PlayerKicked)}>{CallvotePlugin.Instance.Translation.PlayerKicked
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%Player%", player.Nickname)
                        .Replace("%Offender%", ofender.Nickname)
                        .Replace("%Reason%", reason)}</size>",
                        VotingHandler.CurrentVoting.AllowedPlayers);
                }
#if EXILED
                if (ofender.CheckPermission("cv.untouchable"))
                {
                    ofender.Broadcast((ushort)CallvotePlugin.Instance.Config.FinalResultsDuration, CallvotePlugin.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
                }
#else
                if (ofender.HasPermissions("cv.untouchable"))
                {
                    ofender.SendBroadcast(CallvotePlugin.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()), (ushort)CallvotePlugin.Instance.Config.FinalResultsDuration);
                }
#endif
            }
            else
            {
                SoftDependency.MessageProvider.DisplayMessage(
                    TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration),
                    $"<size={DisplayMessageHelper.CalculateMessageSize(CallvotePlugin.Instance.Translation.NotSuccessFullKick)}>{CallvotePlugin.Instance.Translation.NotSuccessFullKick
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKick%", CallvotePlugin.Instance.Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname)}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
        }

        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return CallvotePlugin.Instance.Translation.AskedToKick
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }
    }
}
