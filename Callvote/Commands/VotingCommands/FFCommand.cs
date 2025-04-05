using Callvote.API;
using Callvote.API.Objects;
using CommandSystem;
using System;
using System.Linq;
using Callvote.API.Enums;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;
using CommandSystem.Commands.RemoteAdmin.Broadcasts;

namespace Callvote.Commands.VotingCommands
{
    public class FFCommand : ICommand
    {
        public string Command => "friendlyfire";

        public string[] Aliases => new[] { "ff", "enableff" };

        public string Description => "Calls a enable/disable ff voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableFf)
            {
                response = Callvote.Instance.Translation.VoteFFDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvoteff"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitFf || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.Duration.TotalSeconds}");
                return false;
            }

            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            string question;

            if (!Server.FriendlyFire)
            {
                question = Callvote.Instance.Translation.AskedToDisableFf;
            }
            else
            {
                question = Callvote.Instance.Translation.AskedToEnableFf;
            }

            VotingHandler.CallVoting(
                question
                    .Replace("%Player%", player.Nickname),
                nameof(VotingType.Ff),
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdFf && yesVotePercent > noVotePercent)
                    {
                        switch (Server.FriendlyFire)
                        {
                            case true:
                                {
                                    Server.SendBroadcast(Callvote.Instance.Translation.EnablingFriendlyFire
                                         .Replace("%VotePercent%", yesVotePercent.ToString()), 5);
                                    Server.FriendlyFire = false;
                                    break;
                                }
                            case false:
                                {
                                    Server.SendBroadcast(Callvote.Instance.Translation.DisablingFriendlyFire
                                         .Replace("%VotePercent%", yesVotePercent.ToString()), 5);
                                    Server.FriendlyFire = true;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (Server.FriendlyFire)
                        {
                            case true:
                                {
                                    Server.SendBroadcast(Callvote.Instance.Translation.NoSuccessFullEnableFf
                                        .Replace("%VotePercent%", yesVotePercent.ToString())
                                        .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()), 5);
                                    break;
                                }
                            case false:
                                {
                                    Server.SendBroadcast(Callvote.Instance.Translation.NoSuccessFullDisableFf
                                         .Replace("%VotePercent%", yesVotePercent.ToString())
                                         .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()), 5);
                                    break;
                                }
                        }
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}