using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;

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
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitFf || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitRestartRound - Round.ElapsedTime.TotalSeconds}");
                return false;
            }

            CallvoteAPI.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            CallvoteAPI.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            string question;

            if (!Server.FriendlyFire)
            {
                question = Callvote.Instance.Translation.AskedToDisableFf;
            }
            else
            {
                question = Callvote.Instance.Translation.AskedToEnableFf;
            }

            CallvoteAPI.CallVoting(
                question
                    .Replace("%Player%", player.Nickname),
                nameof(Enums.VotingType.FF),
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
            response = CallvoteAPI.Response;
            return true;
        }
    }
}