#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System;
using System.Collections.Generic;
using Callvote.Features.Interfaces;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Utilities;

namespace Callvote.Features.MessageProviders
{
    /// <summary>
    /// Represents the type that provides HintServiceMeow messages.
    /// </summary>
    public class HSMHintProvider : IMessageProvider
    {
        /// <inheritdoc/>
        public void DisplayMessage(TimeSpan timer, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
                HintServiceMeow.Core.Models.Hints.Hint element = new HintServiceMeow.Core.Models.Hints.Hint
                {
                    Text = content,
                    YCoordinate = RueIYCoordinateToHSMYCoordinate(CallvotePlugin.Instance.Config.HintYCoordinate),
                };
                playerDisplay.AddHint(element);
                playerDisplay.RemoveAfter(element, (float)(timer.TotalSeconds - 0.031));
            }
        }

        private static float RueIYCoordinateToHSMYCoordinate(float rueiPos)
        {
            if (rueiPos < 300f)
            {
                rueiPos = 300f;
            }

            if (rueiPos > 1000f)
            {
                rueiPos = 1000f;
            }

            float t = (rueiPos - 300f) / (1000f - 300f);
            float invertedT = 1f - t;
            return invertedT * 800f;
        }
    }
}
