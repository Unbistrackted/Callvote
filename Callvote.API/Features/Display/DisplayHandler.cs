using System;
using System.Collections.Generic;
using Callvote.API.Enums;
using Callvote.API.Features.Display.DefaultProviders;
using Callvote.API.Features.Generic;
using Callvote.API.Features.Votes;
using UnityEngine;

namespace Callvote.API.Features.Display
{
    /// <summary>
    /// Represents the type that displays the messages during the vote lifecycle, such as the first message with the question and options, the message that updates while vote is active, and the final results message.
    /// </summary>
    public class DisplayHandler : BaseProviderHandler<DisplayProvider>
    {
        private DisplayProvider currentProvider;

        /// <summary>
        /// Gets the current instance of the Display system.
        /// </summary>
        public static DisplayHandler Instance { get; private set; } = new DisplayHandler();

        /// <inheritdoc/>
        public override Dictionary<string, DisplayProvider> Providers { get; } = [];

        /// <inheritdoc/>
        public override DisplayProvider CurrentProvider
        {
            get => this.currentProvider ??= new BroadcastProvider();
            internal set => this.currentProvider = value;
        }

        /// <inheritdoc/>
        public override ProviderType ProviderHandlerType => ProviderType.DisplayMessage;

        /// <summary>
        /// Displays the initial message to <see cref="Vote.AllowedPlayers"/> based on the <see cref="CurrentProvider"/>.
        /// </summary>
        /// <param name="duration">The message duration.</param>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="allowedPlayers">The allowed players that will be able to see this vote.</param>
        public static void Show(float duration, string message, HashSet<ReferenceHub> allowedPlayers)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Instance.CurrentProvider.Show(
                TimeSpan.FromSeconds(Math.Max(0, duration)),
                $"<size={CalculateMessageSize(message)}>{message}</size>",
                allowedPlayers);
        }

        /// <summary>
        /// Calculates the size tag for the message based on its length and Callvote's configuration.
        /// </summary>
        /// <param name="message">The message to have it's size calculated.</param>
        /// <remarks>
        /// I don't really know how I got to those values but they should make everything still be visible when there's too many characters in the message.
        /// </remarks>
        /// <returns>An number for the size tag.</returns>
        public static int CalculateMessageSize(string message)
        {
            int defaultSize = 52;
            int sizeReduction = message.Length / 4;

            if (VoteHandler.CurrentVote.MessageSize != 0)
            {
                defaultSize = VoteHandler.CurrentVote.MessageSize;
                return defaultSize;
            }

            defaultSize -= sizeReduction;

            return Mathf.Clamp(defaultSize, 30, 52);
        }
    }
}
