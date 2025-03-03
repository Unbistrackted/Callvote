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
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class ParentCallVoteCommand : ParentCommand
    {
        public ParentCallVoteCommand()
        {
            LoadGeneratedCommands();
        }
        public override string Command => "callvote";

        public override string[] Aliases => null;

        public override string Description => "";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new KickCommand());
            RegisterCommand(new NukeCommand());
            RegisterCommand(new RespawnWaveCommand());
            RegisterCommand(new RestartRoundCommand());
            RegisterCommand(new RigCommand());
            RegisterCommand(new StopVoteCommand());
            RegisterCommand(new BinaryCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);
            if (player.CheckPermission("cv.callvotecustom") || player.CheckPermission("cv.bypass")) // This exists so people used with the old method doesnt get bothered by the new one
            {
                Dictionary<string, string> options = new Dictionary<string, string>();
                if (args.ToArray().Length == 1)
                {
                    options.Add("yes", Plugin.Instance.Translation.OptionYes);
                    options.Add("no", Plugin.Instance.Translation.OptionNo);
                }
                else
                {
                    string[] optionsArray = args.Skip(1).ToArray();
                    foreach (string option in optionsArray)
                    {
                        if (options.ContainsKey(option))
                        {
                            response = "Its not possible to create a custom vote+command with the same name!";
                            return true;
                        }
                        options.Add(option,option);
                    }
                }
                VoteHandler.StartVote(Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", args.ToArray()[0]), options, null);
                response = "Done";
                return true;
            }
            response = "Wrong Sintax, please use .callvote help";
            return true;
        }
    }
}

