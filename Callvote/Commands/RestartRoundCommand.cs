using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
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

            if (!player.CheckPermission("cv.callvoterestartround"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitRestartRound || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            VotingAPI.CurrentVoting = new Voting(Callvote.Instance.Translation.AskedToRestart
                    .Replace("%Player%", player.Nickname),
                VotingAPI.Options,
                player,
                delegate(Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdRestartRound && yesVotePercent > noVotePercent)
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.RoundRestarting
                            .Replace("%VotePercent%", yesVotePercent.ToString()));
                        Round.Restart();
                    }
                    else
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullRestart
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()));
                    }
                });
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}