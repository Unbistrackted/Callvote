﻿using Callvote.Enums;
using Callvote.Features;
using Callvote.Interfaces;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
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
            vote =>
            {
                int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
                if (yesVotePercent >= Callvote.Instance.Config.ThresholdKill && yesVotePercent > noVotePercent)
                {
                    if (!ofender.CheckPermission("cv.untouchable"))
                    {
                        ofender.Kill(reason);
                        Map.Broadcast(8, Callvote.Instance.Translation.PlayerKilled
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%Player%", player.Nickname)
                            .Replace("%Offender%", ofender.Nickname)
                            .Replace("%Reason%", reason));
                    }
                    if (!ofender.CheckPermission("cv.untouchable")) ofender.Kill(reason);
                    if (ofender.CheckPermission("cv.untouchable")) ofender.Broadcast(5, Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
                }
                else
                {
                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullKill
                        .Replace("%VotePercent%", yesVotePercent.ToString())
                        .Replace("%ThresholdKill%", Callvote.Instance.Config.ThresholdKick.ToString())
                        .Replace("%Offender%", ofender.Nickname));
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
