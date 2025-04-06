using Callvote.API;
using System;
using System.Collections.Generic;

namespace Callvote.Features
{
    public static class DisplayMessageHelper
    {
        public static void DisplayFirstMessage(out string firstMessage)
        {
            firstMessage = Callvote.Instance.Translation.AskedQuestion.Replace("%Question%", VotingHandler.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                if (counter == 0)
                {
                    firstMessage += $"|  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                }
                else
                {
                    firstMessage += $"  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";
                }
                counter++;
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateMessageSize(firstMessage)}>{firstMessage}</size>");
        }

        public static void DisplayWhileVotingMessage(string firstMessage)
        {
            string timerMessage = firstMessage + "\n";
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                timerMessage += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(1), $"<size={CalculateMessageSize(timerMessage)}>{timerMessage}</size>");
        }

        public static void DisplayResultsMessage()
        {
            string resultsMessage = Callvote.Instance.Translation.Results;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                resultsMessage += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateMessageSize(resultsMessage)}>{resultsMessage}</size>");
        }

        private static int CalculateMessageSize(string message)
        {
            int textsize = message.Length / 10;
            if (Callvote.Instance.Config.MessageSize != 0)
            {
                textsize = 52 - Callvote.Instance.Config.MessageSize;
                return textsize;
            }
            return textsize;
        }
    }
}
