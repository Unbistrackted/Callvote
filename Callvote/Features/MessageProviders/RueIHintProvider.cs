#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features.Interfaces;
using RueI.API;
using RueI.API.Elements;

namespace Callvote.Features.MessageProviders
{
    /// <summary>
    /// Represents the type that provides RueI messages.
    /// </summary>
    public class RueIHintProvider : IMessageProvider
    {
        /// <inheritdoc/>
        public void DisplayMessage(TimeSpan timer, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                Tag tag = new("Callvote-Ruei");
                BasicElement element = new(CallvotePlugin.Instance.Config.HintYCoordinate, content);
                RueDisplay display = RueDisplay.Get(player.ReferenceHub);
                display.Remove(tag);
                display.Show(tag, element, timer);
            }
        }
    }
}
