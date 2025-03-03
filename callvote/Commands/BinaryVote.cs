﻿using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new[] { "binario", "bi", "b" };

        public string Description => "Calls a binary voting";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            var options = new Dictionary<string, string>();

            var player = Player.Get((CommandSender)sender);


            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
            options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);

            VoteAPI.StartVote(
                Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname)
                    .Replace("%Custom%", args.ElementAt(0)), options, null);

            response = Plugin.Instance.Translation.VoteStarted;
            return true;
        }
    }
}