using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class CustomVotingCommand : ICommand
    {
        public string Command => "custom";

        public string[] Aliases => new[] { "c" };

        public string Description => "Calls a custom voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }

            if (args.Count < 2)
            {
                response = Plugin.Instance.Translation.LessThanTwoOptions;
                return false;
            }

            foreach (var option in args.Skip(1))
            {
                if (options.ContainsKey(option))
                {
                    response = Plugin.Instance.Translation.DuplicateCommand;
                    return false;
                }

                options.Add(option, option);
            }
            VotingAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", args.ElementAt(0)), options, player, null);
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }
    }
}