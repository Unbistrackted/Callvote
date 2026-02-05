using Callvote.API;
using System;
using System.Collections.Generic;

namespace Callvote.Features
{
    public static class DisplayMessageHelper
    {
        public static void DisplayFirstMessage(string question, out string firstMessage)
        {
            firstMessage = Callvote.Instance.Translation.AskedQuestion.Replace("%Question%", question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                if (counter == 0)
                    firstMessage += $"|  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                else
                    firstMessage += $"  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";

                counter++;
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateMessageSize(firstMessage)}>{firstMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        public static void DisplayWhileVotingMessage(string firstMessage)
        {
            string timerMessage = firstMessage + "\n";

            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
                timerMessage += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());

            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.RefreshInterval), $"<size={CalculateMessageSize(timerMessage)}>{timerMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        public static void DisplayResultsMessage()
        {
            string resultsMessage = Callvote.Instance.Translation.Results;

            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
                resultsMessage += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());

            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(Callvote.Instance.Config.FinalResultsDuration), $"<size={CalculateMessageSize(resultsMessage)}>{resultsMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        public static int CalculateMessageSize(string message)
        {
            int sizeReduction = message.Length / 4;
            int defaultSize = 52;

            if (Callvote.Instance.Config.MessageSize != 0)
            {
                sizeReduction = Callvote.Instance.Config.MessageSize;
                return sizeReduction;
            }

            defaultSize -= sizeReduction;

            if (defaultSize < 30)
                defaultSize = 30;

            return defaultSize;
        }
    }
}
