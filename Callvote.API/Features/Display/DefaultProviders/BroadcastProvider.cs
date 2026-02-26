using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers;

namespace Callvote.API.Features.Display.DefaultProviders
{
    /// <summary>
    /// Represents the type that provides broadcasts messages.
    /// </summary>
    public class BroadcastProvider : DisplayProvider
    {
        /// <inheritdoc/>
        public override string Name => "Callvote.API.Broadcast";

        /// <inheritdoc/>
        public override void Show(TimeSpan duration, string content, HashSet<ReferenceHub> players)
        {
            foreach (ReferenceHub player in players)
            {
                Server.SendBroadcast(Player.Get(player), message: content, duration: (ushort)duration.TotalSeconds);
            }
        }
    }
}
