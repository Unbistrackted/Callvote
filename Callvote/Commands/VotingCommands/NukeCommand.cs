using Callvote.API;
using Callvote.API.Objects;
using CommandSystem;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using System;
using System.Linq;
using Callvote.API.Enums;

namespace Callvote.Commands.VotingCommands
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

            if (!player.HasPermissions("cv.callvotenuke"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitNuke || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitNuke - Round.Duration.TotalSeconds}");
                return false;
            }

            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);


            VotingHandler.CallVoting(
                Callvote.Instance.Translation.AskedToNuke
                    .Replace("%Player%", player.Nickname),
                nameof(VotingType.Nuke),
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdNuke && yesVotePercent > noVotePercent)
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.FoundationNuked
                            .Replace("%VotePercent%", yesVotePercent.ToString()), 5);
                        Warhead.Start();
                    }
                    else
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.NoSuccessFullNuke
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdNuke%", Callvote.Instance.Config.ThresholdNuke.ToString()), 5);
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}