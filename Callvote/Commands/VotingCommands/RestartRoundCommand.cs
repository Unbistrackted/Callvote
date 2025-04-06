using Callvote.API;
using Callvote.Features;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;
using System.Linq;
using Callvote.Enums;

namespace Callvote.Commands.VotingCommands
{
    public class RestartRoundCommand : ICommand
    {
        public string Command => "restartround";

        public string[] Aliases => new[] { "restart", "rround" };

        public string Description => "Calls a restart round voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableRoundRestart)
            {
                response = Callvote.Instance.Translation.VoteRestartRoundDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvoterestartround"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitRestartRound || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.Duration.TotalSeconds}");
                return false;
            }

            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            VotingHandler.CallVoting(
                Callvote.Instance.Translation.AskedToRestart
                    .Replace("%Player%", player.Nickname),
                nameof(VotingType.RestartRound),
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.RoundRestarting
                            .Replace("%VotePercent%", yesVotePercent.ToString()), 5);
                        Round.Restart();
                    }
                    else
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.NoSuccessFullRestart
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()), 5);
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}