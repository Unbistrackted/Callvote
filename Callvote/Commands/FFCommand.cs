using System;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
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

            if (!player.CheckPermission("cv.callvoteff"))
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

            string question;

            if (!Server.FriendlyFire) 
            {
                question = Callvote.Instance.Translation.AskedToDisableFf; 
            } 
            else 
            { 
                question = Callvote.Instance.Translation.AskedToEnableFf;
            }

            VotingAPI.CurrentVoting = new Voting(question
                    .Replace("%Player%", player.Nickname),
                VotingAPI.Options,
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f);
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdFF && yesVotePercent > noVotePercent)
                    {
                        switch (Server.FriendlyFire)
                        {
                            case true:
                                {
                                    Map.Broadcast(5, Callvote.Instance.Translation.EnablingFriendlyFire
                                         .Replace("%VotePercent%", yesVotePercent.ToString()));
                                    Server.FriendlyFire = false;
                                    break;
                                }
                            case false:
                                {
                                    Map.Broadcast(5, Callvote.Instance.Translation.DisablingFriendlyFire
                                         .Replace("%VotePercent%", yesVotePercent.ToString()));
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
                                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullEnableFf
                                        .Replace("%VotePercent%", yesVotePercent.ToString())
                                        .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()));
                                    break;
                                }
                            case false:
                                {
                                    Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullDisableFf
                                         .Replace("%VotePercent%", yesVotePercent.ToString())
                                         .Replace("%ThresholdRestartRound%", Callvote.Instance.Config.ThresholdRestartRound.ToString()));
                                    break;
                                }
                        }
                    }
                });
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}