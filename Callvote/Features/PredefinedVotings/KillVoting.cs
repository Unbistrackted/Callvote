#if EXILED
using Exiled.API.Features;
#else
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using Callvote.API.VotingsTemplate;
using Callvote.Configuration;
using Callvote.Features.Enums;
using Callvote.Features.Interfaces;

namespace Callvote.Features.PredefinedVotings
{
    /// <summary>
    /// Represents the type for the Kill Player Predefined Voting.
    /// Initializes a new instance of the <see cref="KillVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    /// <param name="ofender">The <see cref="Player"/> that is going to be killed.</param>
    /// <param name="reason">The reason for the kick.</param>
    public class KillVoting(Player player, Player ofender, string reason) : BinaryVoting(player, ReplacePlayer(player, ofender, reason), nameof(VotingTypeEnum.Kill), vote => AddCallback(vote, player, ofender, reason)), IVotingTemplate
    {
        private static readonly Translation Translation = CallvotePlugin.Instance.Translation;
        private static readonly Config Config = CallvotePlugin.Instance.Config;

        private static void AddCallback(Voting voting, Player player, Player ofender, string reason)
        {
            if (voting is not BinaryVoting binaryVoting)
            {
                return;
            }

            int yesVotePercent = voting.GetVotePercentage(binaryVoting.YesVote);
            int noVotePercent = voting.GetVotePercentage(binaryVoting.NoVote);

            string message;

            if (yesVotePercent >= Config.ThresholdKill && yesVotePercent > noVotePercent)
            {
                message = Translation.PlayerKilled
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", ofender.Nickname)
                    .Replace("%Reason%", reason);

                ofender.Kill(reason);
            }
            else
            {
                message = Translation.NoSuccessFullKill
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdKill%", Config.ThresholdKick.ToString())
                    .Replace("%Offender%", ofender.Nickname);
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                voting.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player, Player offender, string reason)
        {
            return Translation.AskedToKill
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", offender.Nickname)
                    .Replace("%Reason%", reason);
        }
    }
}
