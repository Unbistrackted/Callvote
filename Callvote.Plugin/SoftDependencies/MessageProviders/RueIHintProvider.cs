#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System;
using System.Collections.Generic;
using Callvote.API.Features.Display;
using RueI.API;
using RueI.API.Elements;

namespace Callvote.SoftDependencies.MessageProviders
{
    /// <summary>
    /// Represents the type that provides RueI messages.
    /// </summary>
    public class RueIHintProvider : DisplayProvider
    {
        /// <inheritdoc/>
        public override string Name => "Callvote.RueI";

        /// <inheritdoc/>
        public override void Show(TimeSpan timer, string content, HashSet<ReferenceHub> players)
        {
            foreach (ReferenceHub player in players)
            {
                Tag tag = new("Callvote-Ruei");
                BasicElement element = new(CallvotePlugin.Instance.Config.HintYCoordinate, content);
                RueDisplay display = RueDisplay.Get(player);
                display.Remove(tag);
                display.Show(tag, element, timer);
            }
        }
    }
}
