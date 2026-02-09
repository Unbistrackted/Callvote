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
using Callvote.Features.PredefinedVotes;
using CommandSystem;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class KillCommand : ICommand
    {
        public string Command => "kill";

        public string[] Aliases => ["death", "kil"];

        public string Description => "Calls a kill vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!CallvotePlugin.Instance.Config.EnableKill)
            {
                response = CallvotePlugin.Instance.Translation.VoteKillDisabled;
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.callvotekill") && player != null)
#else
            if (!player.HasPermissions("cv.callvotekill") && player != null)
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (args.Count == 0)
            {
                response = "callvote Kill [player] [reason]";
                return false;
            }
#if EXILED
            if (!player.CheckPermission("cv.bypass") && Round.ElapsedTime.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitKill)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitKill - Round.ElapsedTime.TotalSeconds:F0}");
#else
            if (!player.HasPermissions("cv.bypass") && Round.Duration.TotalSeconds < CallvotePlugin.Instance.Config.MaxWaitKill)
            {
                response = CallvotePlugin.Instance.Translation.WaitToVote.Replace("%Timer%", $"{CallvotePlugin.Instance.Config.MaxWaitKill - Round.Duration.TotalSeconds:F0}");
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

#if EXILED
            if (!locatedPlayer.CheckPermission("cv.untouchable"))
#else
            if (!locatedPlayer.HasPermissions("cv.untouchable"))
#endif
            {
                response = "Player is Untouchable! :trollface:";
                return false;
            }

            List<Player> playerSearch = [.. Player.List.Where(p => p.Nickname.Contains(args.ElementAt(0)))];

            if (playerSearch.Count() is < 0 or > 1)
            {
                response = CallvotePlugin.Instance.Translation.PlayersWithSameName.Replace("%Player%", args.ElementAt(0));
                return false;
            }

            string reason = string.Join(" ", args.Skip(1));

            response = VoteHandler.CallVote(new KillVote(player, locatedPlayer, reason));
            return true;
        }
    }
}