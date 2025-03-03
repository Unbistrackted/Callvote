using System;
using System.Collections.Generic;
using System.Linq;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Callvote.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ParentCallVoteCommand : ParentCommand
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
            RegisterCommand(new CustomVoteCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            var player = Player.Get(sender);
            if (player.CheckPermission("cv.callvotecustom"))
            {
                var options = new Dictionary<string, string>();
                if (args.Count == 1)
                {
                    options.Add(Plugin.Instance.Translation.CommandYes, Plugin.Instance.Translation.OptionYes);
                    options.Add(Plugin.Instance.Translation.CommandNo, Plugin.Instance.Translation.OptionNo);
                }
                else
                {
                    foreach (var option in args.Skip(1))
                    {
                        if (options.ContainsKey(option))
                        {
                            response = Plugin.Instance.Translation.DuplicateCommand;
                            return true;
                        }

                        options.Add(option, option);
                    }
                }

                VoteAPI.StartVote(
                    Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname)
                        .Replace("%Custom%", args.ElementAt(0)), options, null);
                response = Plugin.Instance.Translation.VoteStarted;
                return true;
            }

            response = "Wrong Syntax, please use .callvote help";
            return true;
        }
    }
}