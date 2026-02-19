#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System;
using System.Collections.Generic;
using Callvote.SoftDependencies.Interfaces;
using LabApi.Features.Wrappers;

namespace Callvote.SoftDependencies.MessageProviders
{
    /// <summary>
    /// Represents the type that provides broadcasts messages.
    /// </summary>
    public class BroadcastProvider : IMessageProvider
    {
        /// <inheritdoc/>
        public void Show(TimeSpan duration1, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                Server.SendBroadcast(player, message: content, duration: (ushort)duration1.TotalSeconds);
            }
        }
    }
}
