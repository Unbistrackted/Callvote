using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class NukeCommand : ICommand
    {
        public string Command => "nuke";

        public string[] Aliases => new[] { "nu", "bomba" };

        public string Description => "Calls a nuke voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableNuke)
            {
                response = Callvote.Instance.Translation.VoteNukeDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotenuke"))
            {
                response = Callvote.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingAPI.Options.Add(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            VotingAPI.CurrentVoting = new Voting(Callvote.Instance.Translation.AskedToNuke
                    .Replace("%Player%", player.Nickname),
                VotingAPI.Options,
                player,
                delegate(Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.FoundationNuked
                            .Replace("%VotePercent%", yesVotePercent.ToString()));
                        Warhead.Start();
                    }
                    else
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullNuke
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdNuke%", Callvote.Instance.Config.ThresholdNuke.ToString()));
                    }
                });
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}