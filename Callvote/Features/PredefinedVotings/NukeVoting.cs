#if EXILED
using Exiled.API.Features;
#else
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
    /// Represents the type for the enable nuke predefined voting.
    /// Initializes a new instance of the <see cref="NukeVoting"/> class.
    /// </summary>
    /// <param name="player"><see cref="Voting.CallVotePlayer"/>.</param>
    public class NukeVoting(Player player) : BinaryVoting(player, ReplacePlayer(player), nameof(VotingTypeEnum.Nuke), AddCallback), IVotingTemplate
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        private static void AddCallback(Voting voting)
        {
            if (voting is not BinaryVoting binaryVoting)
            {
                return;
            }

            int yesVotePercent = voting.GetVotePercentage(binaryVoting.YesVote);
            int noVotePercent = voting.GetVotePercentage(binaryVoting.NoVote);

            string message;

            if (yesVotePercent >= Config.ThresholdNuke && yesVotePercent > noVotePercent)
            {
                message = Translation.FoundationNuked.Replace("%VotePercent%", yesVotePercent.ToString());

                Warhead.Detonate();
            }
            else
            {
                message = Translation.NoSuccessFullNuke
                    .Replace("%VotePercent%", yesVotePercent.ToString())
                    .Replace("%ThresholdNuke%", Config.ThresholdNuke.ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={DisplayMessageHelper.CalculateMessageSize(message)}>{message}</size>",
                voting.AllowedPlayers);
        }

        private static string ReplacePlayer(Player player)
        {
            return Translation.AskedToNuke.Replace("%Player%", player.Nickname);
        }
    }
}
