using System;
using System.Collections.Generic;
using Callvote.API;
using Callvote.Features.Interfaces;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that displays the messages during the voting lifecycle, such as the first message with the question and options, the message that updates while voting is active, and the final results message.
    /// </summary>
    internal static class DisplayMessageHelper
    {
        /// <summary>
        /// Displays the initial message to <see cref="Voting.AllowedPlayers"/> based on the <see cref="IMessageProvider"/>.
        /// </summary>
        /// <param name="question">The question to be displayed.</param>
        /// <param name="firstMessage">The message that was sent.</param>
        /// <remarks>
        /// This is a behaviour from the legacy Callvote, where it would a wait 5 seconds before actually showing the counter. I kept it to keep the old feeling.
        /// </remarks>
        internal static void DisplayFirstMessage(string question, out string firstMessage)
        {
            firstMessage = CallvotePlugin.Instance.Translation.AskedQuestion.Replace("%Question%", question);
            int counter = 0;
            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                if (counter == 0)
                {
                    firstMessage += $"|  {CallvotePlugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)}  |";
                }
                else
                {
                    firstMessage += $"  {CallvotePlugin.Instance.Translation.Options.Replace("%OptionKey%", kvp.Key).Replace("%Option%", kvp.Value)} |";
                }

                counter++;
            }

            SoftDependency.MessageProvider.DisplayMessage(TimeSpan.FromSeconds(5), $"<size={CalculateMessageSize(firstMessage)}>{firstMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        /// <summary>
        /// Displays the message while <see cref="VotingHandler.IsVotingActive"/> to <see cref="Voting.AllowedPlayers"/> based on the <see cref="IMessageProvider"/>.
        /// </summary>
        /// <param name="firstMessage">The first message sent when the <see cref="Voting"/> started.</param>
        /// <remarks>
        /// This message contains the first message and the options with their respective counters.
        /// </remarks>
        internal static void DisplayWhileVotingMessage(string firstMessage)
        {
            string timerMessage = firstMessage + "\n";

            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                timerMessage += CallvotePlugin.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.RefreshInterval), $"<size={CalculateMessageSize(timerMessage)}>{timerMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        /// <summary>
        /// Displays the results message based on <see cref="Voting.Counter"/> and <see cref="Voting.Options"/>.
        /// </summary>
        internal static void DisplayResultsMessage()
        {
            string resultsMessage = CallvotePlugin.Instance.Translation.Results;

            foreach (KeyValuePair<string, string> kvp in VotingHandler.CurrentVoting.Options)
            {
                resultsMessage += CallvotePlugin.Instance.Translation.OptionAndCounter
                    .Replace("%Option%", kvp.Value)
                    .Replace("%OptionKey%", kvp.Key)
                    .Replace("%Counter%", VotingHandler.CurrentVoting.Counter[kvp.Key].ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(TimeSpan.FromSeconds(CallvotePlugin.Instance.Config.FinalResultsDuration), $"<size={CalculateMessageSize(resultsMessage)}>{resultsMessage}</size>", VotingHandler.CurrentVoting.AllowedPlayers);
        }

        /// <summary>
        /// Calculates the size tag for the message based on its length and Callvote's configuration.
        /// </summary>
        /// <param name="message">The message to have it's size calculated.</param>
        /// <remarks>
        /// I don't really know how I got to those values but they should make everything still be visible when there's too many characters in the message.
        /// </remarks>
        /// <returns>An number for the size tag.</returns>
        internal static int CalculateMessageSize(string message)
        {
            int sizeReduction = message.Length / 4;
            int defaultSize = 52;

            if (CallvotePlugin.Instance.Config.MessageSize != 0)
            {
                sizeReduction = CallvotePlugin.Instance.Config.MessageSize;
                return sizeReduction;
            }

            defaultSize -= sizeReduction;

            if (defaultSize < 30)
            {
                defaultSize = 30;
            }

            return defaultSize;
        }
    }
}
