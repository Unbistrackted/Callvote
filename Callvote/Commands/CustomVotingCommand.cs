using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Callvote.VoteHandlers;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using static Exiled.API.Features.DamageHandlers.DamageHandlerBase;

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
            List<string> argsStrings = JoinWordsBetweenQuotes(args);
            List<string> optionDetailsStrings = ExtractAndRemoveParenthesesValues(ref argsStrings);
            Player player = Player.Get(sender);

            if (!player.CheckPermission("cv.callvotecustom") || !player.CheckPermission("cv.bypass"))
            {
                response = Plugin.Instance.Translation.NoPermissionToVote;
                return false;
            }


            if (argsStrings.Count < 2)
            {
                response = Plugin.Instance.Translation.LessThanTwoOptions;
                return false;
            }

            for (int i = 1; i < argsStrings.Count; i++) // args = 3
            {
                string optionDetail;
                if (!optionDetailsStrings.TryGet(i-1, out optionDetail))
                {
                    optionDetail = argsStrings[i];
                }
                options.Add(argsStrings[i], optionDetail);
            }
            VotingAPI.CurrentVoting = new Voting(Plugin.Instance.Translation.AskedCustom.Replace("%Player%", player.Nickname).Replace("%Custom%", argsStrings.First()), options, player, null);
            response = VotingAPI.CurrentVoting.Response;
            return true;
        }

        static List<string> JoinWordsBetweenQuotes(ArraySegment<string> words)
        {
            List<string> finalList = new List<string>();
            bool insideQuotes = false;
            List<string> wordsBetweenQuotes = new List<string>();

            foreach (string word in words)
            {
                if (word.StartsWith("\""))
                {
                    insideQuotes = true;
                    wordsBetweenQuotes.Clear(); // Start new group
                }

                if (insideQuotes)
                {
                    wordsBetweenQuotes.Add(word.Trim('"'));

                    if (word.EndsWith("\"") && word.Length > 1)
                    {
                        finalList.Add(string.Join(" ", wordsBetweenQuotes));
                        insideQuotes = false;
                    }
                }
                else
                {
                    finalList.Add(word);
                }
            }

            return finalList;
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