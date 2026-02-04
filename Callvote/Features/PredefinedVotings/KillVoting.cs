#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using LabApi.Features.Permissions;
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
    public class KillVoting(Player player, Player ofender, string reason) : BinaryVoting(player, ReplacePlayer(player, ofender, reason), nameof(VotingTypeEnum.Kill), vote => AddCallback(vote, player, ofender, reason)), IVotingTemplate
    {
        public static void AddCallback(Voting vote, Player player, Player ofender, string reason)
        {
            int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
            int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);

            if (yesVotePercent >= Callvote.Instance.Config.ThresholdKill && yesVotePercent > noVotePercent)
            {
#if EXILED
                if (!ofender.CheckPermission("cv.untouchable"))
#else
                if (!ofender.HasPermissions("cv.untouchable"))
#endif
                {
                    ofender.Kill(reason);
                    MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.PlayerKilled)}>{Callvote.Instance.Translation.PlayerKilled
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%Player%", player.Nickname)
                        .Replace("%Offender%", ofender.Nickname)
                        .Replace("%Reason%", reason)}</size>",
                        VotingHandler.CurrentVoting.AllowedPlayers);
                }
#if EXILED
                if (ofender.CheckPermission("cv.untouchable")) ofender.Broadcast((ushort)Callvote.Instance.Config.FinalResultsDuration, Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
#else
                if (ofender.HasPermissions("cv.untouchable")) ofender.SendBroadcast(Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()), (ushort)Callvote.Instance.Config.FinalResultsDuration);
#endif
            }
            else
            {
                MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={DisplayMessageHelper.CalculateMessageSize(Callvote.Instance.Translation.NoSuccessFullKill)}>{Callvote.Instance.Translation.NoSuccessFullKill
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKill%", Callvote.Instance.Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname)}</size>",
                    VotingHandler.CurrentVoting.AllowedPlayers);
            }
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
