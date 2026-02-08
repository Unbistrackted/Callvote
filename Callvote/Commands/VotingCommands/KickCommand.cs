#if EXILED
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
#else
using Callvote.Commands.ParentCommands;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.API;
using Callvote.Features.PredefinedVotings;
using CommandSystem;

namespace Callvote.Commands.VotingCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteCommand))]
#endif
    public class KickCommand : ICommand
    {
        public string Command => "kick";

        public string[] Aliases => ["ki", "k"];

        public string Description => "Calls a kick voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableKick)
            {
                response = CallvotePlugin.Instance.Translation.VoteKickDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvotekick") && player != null)
#else
            if (!player.HasPermissions("cv.callvotekick") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kick [player] [reason]";
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitKick)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitKick - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitKick)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitKick - Round.Duration.TotalSeconds:F0}");
#endif
                return false;
            }

            if (args.Count == 1)
            {
                response = CallvotePlugin.Instance.Translation.PassReason;
                return false;
            }
#if EXILED
            Player locatedPlayer = Player.Get(args.ElementAt(0));
#else
            Player locatedPlayer = Player.GetByNickname(args.ElementAt(0));
#endif

            if (locatedPlayer == null)
            {
                response = CallvotePlugin.Instance.Translation.PlayerNotFound.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            List<Player> playerSearch = [.. Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0)))];

            if (playerSearch.Count() is < 0 or > 1)
            {
                response = CallvotePlugin.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = string.Join(" ", args.Skip(1));

            response = VotingHandler.CallVoting(new KickVoting(player, locatedPlayer, reason));
            return true;
        }
    }
}