using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Callvote.Commands
{
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => new[] { "death", "kil" };

        public string Description => "Calls a kill voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!Callvote.Instance.Config.EnableKill)
            {
                response = Callvote.Instance.Translation.VoteKillDisabled;
                return false;
            }

            if (!player.CheckPermission("cv.callvotekill"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill <player> (reason)";
                return false;
            }

            if (Round.ElapsedTime.TotalSeconds < Callvote.Instance.Config.MaxWaitKill || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.WaitToVote.Replace("%Timer%", $"{Callvote.Instance.Config.MaxWaitKill - Round.ElapsedTime.TotalSeconds}");
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
                Callvote.Instance.Translation.AskedToKill
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Offender%", locatedPlayer.Nickname)
                    .Replace("%Reason%", reason),
                nameof(Enums.VotingType.Kill),
                player,
                delegate (Voting vote)
                {
                    int yesVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandYes] / (float)Player.List.Count() * 100f);
                    int noVotePercent = (int)(vote.Counter[Callvote.Instance.Translation.CommandNo] / (float)Player.List.Count() * 100f); //Just so you know that it exists
                    if (yesVotePercent >= Callvote.Instance.Config.ThresholdKill && yesVotePercent > noVotePercent)
                    {
                        if (!locatedPlayer.CheckPermission("cv.untouchable"))
                        {
                            locatedPlayer.Kill(reason);
                            Map.Broadcast(8, Callvote.Instance.Translation.PlayerKilled
                                .Replace("%VotePercent%", yesVotePercent.ToString())
                                .Replace("%Player%", player.Nickname)
                                .Replace("%Offender%", locatedPlayer.Nickname)
                                .Replace("%Reason%", reason));
                        }
                        if (!locatedPlayer.CheckPermission("cv.untouchable")) locatedPlayer.Kill(reason);
                        if (locatedPlayer.CheckPermission("cv.untouchable")) locatedPlayer.Broadcast(5, Callvote.Instance.Translation.Untouchable.Replace("%VotePercent%", yesVotePercent.ToString()));
                    }
                    else
                    {
                        Map.Broadcast(5, Callvote.Instance.Translation.NoSuccessFullKill
                            .Replace("%VotePercent%", yesVotePercent.ToString())
                            .Replace("%ThresholdKick%", Callvote.Instance.Config.ThresholdKick.ToString())
                            .Replace("%Offender%", locatedPlayer.Nickname));
                    }
                });
            response = VotingHandler.Response;
            return true;
        }
    }
}