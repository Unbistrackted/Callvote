using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using UnityEngine;

namespace callvote.Commands
{
    public class VoteCommand : ICommand
    {
        public string Command { get; set; }

        public string[] Aliases { get; set; }

        public string Description { get; } = "";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            int x = arguments.Count();

            Player player = Player.Get(((CommandSender)sender).SenderId);

            if (Plugin.Instance.CurrentVote == null)
            {
                response = response = "No vote is in progress.";
                return true;
            }

            response = VoteHandler.Voting(player, Command);
            return true;
        }

        public VoteCommand(string command, string[] aliases)
        {
            Command = command;
            Aliases = aliases;
        }
    }
}
