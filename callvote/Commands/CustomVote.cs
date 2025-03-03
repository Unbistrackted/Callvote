using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class CustomVoteCommand : ICommand
    {
        public string Command => "custom";

        public string[] Aliases => new[] { "c" };

        public string Description => "Calls a custom voting";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            var options = new Dictionary<string, string>();

            var player = Player.Get(sender);


            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            if (args.Count < 2)
            {
                response = Plugin.Instance.Translation.LessThanTwoOptions;
                return true;
            }

            foreach (var option in args.Skip(1))
            {
                if (options.ContainsKey(option))
                {
                    response = Plugin.Instance.Translation.DuplicateCommand;
                    return true;
                }

                options.Add(option, option);
            }

            VoteAPI.StartVote(
                Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname)
                    .Replace("%Custom%", args.ElementAt(0)), options, null);
            response = Plugin.Instance.Translation.VoteStarted;
            return true;
        }
    }
}