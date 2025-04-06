using System;
using System.Collections.Generic;

namespace Callvote.API
{
    public static class DisplayMessageHelper
    {
        public static void DisplayFirstMessage(out string firstBroadcast)
        {
            firstBroadcast = Callvote.Instance.Translation.AskedQuestion.Replace("%Question%", VotingHandler.CurrentVoting.Question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                if (counter == 0)
                {
                    firstBroadcast += $"|  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                }
                else
                {
                    firstBroadcast += $"  {Callvote.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";
                }
                counter++;
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateBroadcastSize(firstBroadcast)}>{firstBroadcast}</size>");
        }

        public static void DisplayWhileVotingMessage(string firstBroadcast)
        {
            string timerBroadcast = firstBroadcast + "\n";
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                timerBroadcast += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(1), $"<size={CalculateBroadcastSize(timerBroadcast)}>{timerBroadcast}</size>");
        }

        public static void DisplayResultsMessage()
        {
            string resultsBroadcast = Callvote.Instance.Translation.Results;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                resultsBroadcast += Callvote.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }
            MessageProvider.Provider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateBroadcastSize(resultsBroadcast)}>{resultsBroadcast}</size>");
        }

        private static int CalculateBroadcastSize(string broadcast)
        {
            int textsize = broadcast.Length / 10;
            if (Callvote.Instance.Config.BroadcastSize != 0)
            {
                textsize = 52 - Callvote.Instance.Config.BroadcastSize;
                return textsize;
            }
            return textsize;
        }
    }
}
