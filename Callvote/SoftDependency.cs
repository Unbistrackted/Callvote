namespace Callvote
{
#if EXILED
    using Player = Exiled.API.Features.Player;
#else
    using Player = LabApi.Features.Wrappers.Player;
#endif
    using HarmonyLib;
    using HintServiceMeow.Core.Extension;
    using HintServiceMeow.Core.Utilities;
    using LabApi.Features.Wrappers;
    using RueI.API;
    using RueI.API.Elements;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MessageProvider
    {
        public static IMessageProvider Provider { get; } = GetProvider();
        private static IMessageProvider GetProvider()
        {
            if (Callvote.Instance.Config.MessageProvider.ToLower() == "auto")
                return AutoProvider();

            if (Callvote.Instance.Config.MessageProvider.ToLower() == "ruei")
            {
                if (TryLoadRueI(out IMessageProvider ruei))
                    return ruei;

                return new BroadcastProvider();
            }

            if (Callvote.Instance.Config.MessageProvider.ToLower() == "hsm")
            {
                if (TryLoadHSM(out IMessageProvider hsm))
                    return hsm;

                return new BroadcastProvider();
            }

            if (Callvote.Instance.Config.MessageProvider.ToLower() is "broadcast" or "bc")
                return new BroadcastProvider();

            return AutoProvider();
        }

        private static IMessageProvider AutoProvider()
        {
            bool IsRueILoaded = TryLoadRueI(out IMessageProvider ruei);
            bool IsHSMLoaded = TryLoadHSM(out IMessageProvider hsm);

            if (IsHSMLoaded && IsRueILoaded)
                return new BroadcastProvider();

            if (IsRueILoaded)
                return ruei;

            if (IsHSMLoaded)
                return hsm;

            return new BroadcastProvider();
        }

        private static bool TryLoadRueI(out IMessageProvider provider)
        {
            if (IsRueiPatched())
            {
                provider = new RueIHintProvider();
                return true;
            }

            provider = null;
            return false;
        }

        private static bool TryLoadHSM(out IMessageProvider provider)
        {
            if (IsHSMPatched())
            {
                provider = new HSMHintProvider();
                return true;
            }

            provider = null;
            return false;
        }

        private static bool IsHSMPatched()
        {
            return Harmony.GetAllPatchedMethods().Select(Harmony.GetPatchInfo).Any(info => info?.Postfixes?.Any(p => p.owner.Contains("HintServiceMeow")) == true || info?.Prefixes?.Any(p => p.owner.Contains("HintServiceMeow")) == true);
        }

        private static bool IsRueiPatched()
        {
            return Harmony.GetAllPatchedMethods().Select(Harmony.GetPatchInfo).Any(info => info?.Postfixes?.Any(p => p.owner.Contains("RueI")) == true || info?.Transpilers?.Any(p => p.owner.Contains("RueI")) == true);
        }

    }

    public interface IMessageProvider
    {
        void DisplayMessage(TimeSpan duration, string content, HashSet<Player> players);
    }

    public class BroadcastProvider : IMessageProvider
    {
        public void DisplayMessage(TimeSpan duration1, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                Server.SendBroadcast(player, message: content, duration: (ushort)duration1.TotalSeconds);
            }
        }
    }

    public class RueIHintProvider : IMessageProvider
    {
        public void DisplayMessage(TimeSpan timer, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                BasicElement element = new BasicElement(Callvote.Instance.Config.HintYCoordinate, content);
                RueDisplay display = RueDisplay.Get(player.ReferenceHub);
                display.Show(element, timer);
            }
        }
    }

    public class HSMHintProvider : IMessageProvider
    {
        public void DisplayMessage(TimeSpan timer, string content, HashSet<Player> players)
        {
            foreach (Player player in players)
            {
                PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
                HintServiceMeow.Core.Models.Hints.Hint element = new HintServiceMeow.Core.Models.Hints.Hint
                {
                    Text = content,
                    YCoordinate = RueIYCoordinateToHSMYCoordinate(Callvote.Instance.Config.HintYCoordinate)
                };
                playerDisplay.AddHint(element);
                playerDisplay.RemoveAfter(element, (float)(timer.TotalSeconds - 0.031));
            }
        }

        private static float RueIYCoordinateToHSMYCoordinate(float rueiPos)
        {
            if (rueiPos < 300f) rueiPos = 300f;
            if (rueiPos > 1000f) rueiPos = 1000f;
            float t = (rueiPos - 300f) / (1000f - 300f);
            float invertedT = 1f - t;
            return invertedT * 800f;
        }
    }
}