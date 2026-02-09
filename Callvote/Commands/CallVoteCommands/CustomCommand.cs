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
using System.Text.RegularExpressions;
using Callvote.API;
using Callvote.API.VoteTemplate;
using Callvote.Features.Enums;
using CommandSystem;

namespace Callvote.Commands.CallVoteCommands
{
#if !EXILED
    [CommandHandler(typeof(CallVoteParentCommand))]
#endif
    public class CustomCommand : ICommand
    {
        private static readonly Regex CommandDetailRegex = new(@"^(\w+)\(([^)]+)\)$");

        public string Command => "custom";

        public string[] Aliases => ["c"];

        public string Description => "Calls a custom vote.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            List<string> separatedArgs = JoinWordsBetweenQuotes(args);
            Player player = Player.Get(sender);

#if EXILED
            if (player == null || !player.CheckPermission("cv.callvotecustom"))
#else
            if (player == null || !player.HasPermissions("cv.callvotecustom"))
#endif
            {
                response = CallvotePlugin.Instance.Translation.NoPermission;
                return false;
            }

            if (separatedArgs.Count < 2)
            {
                response = CallvotePlugin.Instance.Translation.LessThanTwoOptions;
                return false;
            }

            for (int i = 1; i < separatedArgs.Count; i++)
            {
                string arg = separatedArgs[i];
                Match match = CommandDetailRegex.Match(arg);

                if (!match.Success)
                {
                    response = $"Invalid format: {arg}";
                    return false;
                }

                string command = match.Groups[1].Value;
                string detail = match.Groups[2].Value;

                VoteHandler.CreateVoteOption(command, detail, out _);
            }

            response = VoteHandler.CallVote(new CustomVote(player, CallvotePlugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", separatedArgs.First()), nameof(VoteTypeEnum.Custom)));
            return true;
        }

        private static List<string> JoinWordsBetweenQuotes(IEnumerable<string> args)
        {
            List<string> result = [];
            bool inQuotes = false;
            int parenthesesDepth = 0;

            List<string> buffer = [];

            foreach (string arg in args)
            {
                if (arg.StartsWith("\""))
                {
                    inQuotes = true;
                    buffer.Clear();
                }

                if (arg.Contains("("))
                {
                    parenthesesDepth += arg.Count(c => c == '(');
                    buffer.Clear();
                }

                if (inQuotes || parenthesesDepth > 0)
                {
                    buffer.Add(arg.Trim('"'));

                    if (arg.EndsWith("\"") && inQuotes)
                    {
                        inQuotes = false;
                    }

                    if (arg.Contains(")"))
                    {
                        parenthesesDepth -= arg.Count(c => c == ')');
                    }

                    if (!inQuotes && parenthesesDepth == 0)
                    {
                        result.Add(string.Join(" ", buffer));
                        buffer.Clear();
                    }
                }
                else
                {
                    result.Add(arg);
                }
            }

            return result;
        }
    }
}
