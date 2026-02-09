using System;
using Callvote.API;
using Callvote.Configuration;
using Callvote.Features.Interfaces;
using UnityEngine;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that displays the messages during the vote lifecycle, such as the first message with the question and options, the message that updates while vote is active, and the final results message.
    /// </summary>
    internal static class DisplayMessageHelper
    {
        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        /// <summary>
        /// Displays the initial message to <see cref="Vote.AllowedPlayers"/> based on the <see cref="IMessageProvider"/>.
        /// </summary>
        /// <param name="question">The question to be displayed.</param>
        /// <param name="firstMessage">The message that was sent.</param>
        /// <remarks>
        /// This is a behaviour from the legacy Callvote, where it would a wait 5 seconds before actually showing the counter. I kept it to keep the old feeling.
        /// </remarks>
        internal static void DisplayFirstMessage(string question, out string firstMessage)
        {
            if (!VoteHandler.CurrentVote.CanShowMessages)
            {
                firstMessage = string.Empty;
                return;
            }

            firstMessage = VoteHandler.CurrentVote.ShouldOnlyShowQuestionAndCounter ? question : Translation.AskedQuestion.Replace("%Question%", question);

            int counter = 0;
            foreach (VoteOption vote in VoteHandler.CurrentVote.VoteOptions)
            {
                if (counter == 0)
                {
                    firstMessage += $"|  {Translation.Options.Replace("%VoteCommand%", vote.Command.Command).Replace("%VoteDetail%", vote.Detail)}  |";
                }
                else
                {
                    firstMessage += $"  {Translation.Options.Replace("%VoteCommand%", vote.Command.Command).Replace("%VoteDetail%", vote.Detail)} |";
                }

                counter++;
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(5),
                $"<size={CalculateMessageSize(firstMessage)}>{firstMessage}</size>",
                VoteHandler.CurrentVote.AllowedPlayers);
        }

        /// <summary>
        /// Displays the message while <see cref="VoteHandler.IsVoteActive"/> to <see cref="Vote.AllowedPlayers"/> based on the <see cref="IMessageProvider"/>.
        /// </summary>
        /// <param name="firstMessage">The first message sent when the <see cref="Vote"/> started.</param>
        /// <remarks>
        /// This message contains the first message and the options with their respective counters.
        /// </remarks>
        internal static void DisplayWhileVoteMessage(string firstMessage)
        {
            if (!VoteHandler.CurrentVote.CanShowMessages)
            {
                return;
            }

            string timerMessage = firstMessage + "\n";

            foreach (VoteOption vote in VoteHandler.CurrentVote.VoteOptions)
            {
                timerMessage += Translation.OptionAndCounter
                    .Replace("%VoteDetail%", vote.Detail)
                    .Replace("%VoteCounter%", VoteHandler.CurrentVote.Counter[vote].ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.RefreshInterval),
                $"<size={CalculateMessageSize(timerMessage)}>{timerMessage}</size>",
                VoteHandler.CurrentVote.AllowedPlayers);
        }

        /// <summary>
        /// Displays the results message based on <see cref="Vote.Counter"/> and <see cref="Vote.VoteOptions"/>.
        /// </summary>
        internal static void DisplayResultsMessage()
        {
            if (!VoteHandler.CurrentVote.CanShowMessages)
            {
                return;
            }

            string resultsMessage = Translation.Results;

            foreach (VoteOption vote in VoteHandler.CurrentVote.VoteOptions)
            {
                resultsMessage += Translation.OptionAndCounter
                    .Replace("%VoteDetail%", vote.Detail)
                    .Replace("%VoteCounter%", VoteHandler.CurrentVote.Counter[vote].ToString());
            }

            SoftDependency.MessageProvider.DisplayMessage(
                TimeSpan.FromSeconds(Config.FinalResultsDuration),
                $"<size={CalculateMessageSize(resultsMessage)}>{resultsMessage}</size>",
                VoteHandler.CurrentVote.AllowedPlayers);
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
            int defaultSize = 52;
            int sizeReduction = message.Length / 4;

            if (Config.MessageSize != 0)
            {
                defaultSize = Config.MessageSize;
                return defaultSize;
            }

            defaultSize -= sizeReduction;

            return Mathf.Clamp(defaultSize, 30, 52);
        }
    }
}
