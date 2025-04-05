using Callvote.API;
using Callvote.API.Objects;
using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API.Enums;
using LabApi.Features.Wrappers;
using LabApi.Features.Permissions;

namespace Callvote.Commands.VotingCommands
{
    public class KickCommand : ICommand
    {
        public string Command => "kick";

        public string[] Aliases => new[] { "ki", "k" };

        public string Description => "Calls a kick voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {

            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableKick)
            {
                response = Callvote.Instance.Translation.VoteKickDisabled;
                return false;
            }

            if (!player.HasPermissions("cv.callvotekick"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick <player> (reason)";
                return false;
            }

            if (Round.Duration.TotalSeconds < Callvote.Instance.Config.MaxWaitKick || !player.HasPermissions("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKick - Round.Duration.TotalSeconds}");
                return false;
            }

            if (args.Count == 1)
            {
                response = Callvote.Instance.Translation.PassReason;
                return false;
            }

            Player locatedPlayer = Player.Get(args.ElementAt(0));

            if (locatedPlayer == null)
            {
                response = Callvote.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            List<Player> playerSearch = Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0))).ToList();
            if (playerSearch.Count() < 0 || playerSearch.Count() > 1)
            {
                response = Callvote.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = args.ElementAt(1);

            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandYes, Callvote.Instance.Translation.OptionYes);
            VotingHandler.AddOptionToVoting(Callvote.Instance.Translation.CommandNo, Callvote.Instance.Translation.OptionNo);

            VotingHandler.CallVoting(
                Callvote.Instance.Translation.AskedToKick
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", locatedPlayer.Nickname)
                    .Replace("%Reason%", reason),
                nameof(VotingType.Kick),
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdKick && yesVotePercent > noVotePercent)
                    {
                        if (!locatedPlayer.HasPermissions("cv.untouchable"))
                        {
                            locatedPlayer.Kick(reason);
                            Server.SendBroadcast(Callvote.Instance.Translation.PlayerKicked
                                .Replace("%VotePercent%", yesVotePercent.ToString())
                                .Replace("%Player%", player.Nickname)
                                .Replace("%Offender%", locatedPlayer.Nickname)
                                .Replace("%Reason%", reason), 5);
                        }
                        if (locatedPlayer.HasPermissions("cv.untouchable")) locatedPlayer.SendBroadcast(Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()), 5);
                    }
                    else
                    {
                        Server.SendBroadcast(Callvote.Instance.Translation.NotSuccessFullKick
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdKick%", Callvote.Instance.Config.ThresholdKick.ToString())
                            .Replace("%Offender%", locatedPlayer.Nickname), 5);
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}