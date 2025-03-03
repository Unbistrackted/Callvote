using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using UnityEngine;
using System.Text.RegularExpressions;
using callvote.VoteHandlers;
using MEC;

namespace callvote.Commands
{
    public class BinaryCommand : ICommand
    {
        public string Command => "binary";

        public string[] Aliases => new string[] { "binario", "bi", "b" };

        public string Description => "Calls a voting for restarting the round";
        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            Player player = Player.Get((CommandSender)sender);


            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return true;
            }

            options.Add("yes", Plugin.Instance.Translation.OptionYes);
            options.Add("no", Plugin.Instance.Translation.OptionNo);

            VoteHandler.StartVote(Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", args.ToArray()[0]), options, null);

            response = "Vote started.";
            return true;
        }
    }

}
