using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Callvote.Commands
{
    public class CustomVotingCommand : ICommand
    {
        public string Command => "custom";

        public string[] Aliases => new[] { "c" };

        public string Description => "Calls a custom voting.";

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            List<string> argsStrings = JoinWordsBetweenQuotes(args);
            List<string> optionDetailsStrings = ExtractAndRemoveParenthesesValues(ref argsStrings);
            optionDetailsStrings = JoinWordsBetweenQuotes(optionDetailsStrings);
            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Callvote.Instance.Translation.NoPermission;
                return false;
            }


            if (argsStrings.Count < 2)
            {
                response = Callvote.Instance.Translation.LessThanTwoOptions;
                return false;
            }

            for (int i = 1; i < argsStrings.Count; i++)
            {
                string optionDetail;
                if (!optionDetailsStrings.TryGet(i - 1, out optionDetail))
                {
                    optionDetail = argsStrings[i];
                }
                if (VotingHandler.Options.ContainsKey(argsStrings[i]))
                {
                    response = Callvote.Instance.Translation.DuplicateCommand;
                    return false;
                }
                VotingHandler.AddOptionToVoting(argsStrings[i], optionDetail);
            }
            VotingHandler.CallVoting(
                Callvote.Instance.Translation.AskedCustom
                    .Replace("%Player%", player.Nickname)
                    .Replace("%Custom%", argsStrings.First()),
                nameof(Enums.VotingType.Custom),
                player,
                null);
            response = VotingHandler.Response;
            return true;
        }

        static List<string> JoinWordsBetweenQuotes(ArraySegment<string> args)
        {
            List<string> list = new List<string>();
            bool isInsideQuotes = false;
            List<string> wordsBetweenQuotes = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("\""))
                {
                    isInsideQuotes = true;
                    wordsBetweenQuotes.Clear();
                }

                if (isInsideQuotes)
                {
                    wordsBetweenQuotes.Add(arg.Trim('"'));

                    if (arg.EndsWith("\"") && arg.Length > 1)
                    {
                        list.Add(string.Join(" ", wordsBetweenQuotes));
                        isInsideQuotes = false;
                    }
                }
                else
                {
                    list.Add(arg);
                }
            }
            return list;
        }

        static List<string> JoinWordsBetweenQuotes(List<string> args)
        {
            List<string> list = new List<string>();
            bool isInsideQuotes = false;
            List<string> argsBetweenQuotes = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("\""))
                {
                    isInsideQuotes = true;
                    argsBetweenQuotes.Clear();
                }

                if (isInsideQuotes)
                {
                    argsBetweenQuotes.Add(arg.Trim('"'));

                    if (arg.EndsWith("\"") && arg.Length > 1)
                    {
                        list.Add(string.Join(" ", argsBetweenQuotes));
                        isInsideQuotes = false;
                    }
                }
                else
                {
                    list.Add(arg);
                }
            }
            return list;
        }

        static List<string> ExtractAndRemoveParenthesesValues(ref List<string> list)
        {
            List<string> parenthesesList = new List<string>();
            Regex regex = new Regex(@"\(([^)]+)\)");

            List<string> cleanedList = new List<string>();

            foreach (string item in list)
            {
                MatchCollection matches = regex.Matches(item);
                string cleanedItem = item;

                foreach (Match match in matches)
                {
                    parenthesesList.Add(match.Groups[1].Value);
                    cleanedItem = cleanedItem.Replace(match.Value, "").Trim();
                }

                if (!string.IsNullOrEmpty(cleanedItem))
                {
                    cleanedList.Add(cleanedItem);
                }
            }

            list.Clear();
            list.AddRange(cleanedList);

            return parenthesesList;
        }
    }
}