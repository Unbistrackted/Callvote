﻿namespace Callvote
{
    using Exiled.API.Features;
    using HarmonyLib;
    using HintServiceMeow.Core.Extension;
    using HintServiceMeow.Core.Utilities;
    using RueI;
    using RueI.Displays;
    using RueI.Displays.Scheduling;
    using RueI.Elements;
    using RueI.Extensions;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class MessageProvider
    {
        public static IMessageProvider Provider { get; } = GetProvider();
        private static IMessageProvider GetProvider()
        {
            if (Callvote.Instance.Config.MessageProvider.ToLower() == "auto")
            {
                return AutoProvider();
            }
            if (Callvote.Instance.Config.MessageProvider.ToLower() == "ruei")
            {
                bool IsRueILoaded = TryLoadRueI(out IMessageProvider ruei);

                if (IsRueILoaded)
                {
                    return ruei;
                }

                return new BroadcastProvider();
            }
            if (Callvote.Instance.Config.MessageProvider.ToLower() == "hsm")
            {
                bool IsHSMLoaded = TryLoadHSM(out IMessageProvider hsm);

                if (IsHSMLoaded)
                {
                    return hsm;
                }

                return new BroadcastProvider();
            }
            if (Callvote.Instance.Config.MessageProvider.ToLower() == "broadcast" || Callvote.Instance.Config.MessageProvider.ToLower() == "bc")
            {
                return new BroadcastProvider();
            }
            return AutoProvider();
        }

        private static IMessageProvider AutoProvider()
        {
            bool IsRueILoaded = TryLoadRueI(out IMessageProvider ruei);
            bool IsHSMLoaded = TryLoadHSM(out IMessageProvider hsm);

            if (IsHSMLoaded && IsRueILoaded)
            {
                return new BroadcastProvider();
            }
            if (IsRueILoaded)
            {
                return ruei;
            }
            if (IsHSMLoaded)
            {
                return hsm;
            }

            return new BroadcastProvider();
        }

        private static bool TryLoadRueI(out IMessageProvider provider)
        {
            try
            {
                LoadRueI();
                provider = new RueIHintProvider();
                return true;
            }
            catch
            {
                provider = null;
                return false;
            }
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


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadRueI() => RueIMain.EnsureInit();

    }
    public interface IMessageProvider
    {
        void DisplayMessage(TimeSpan duration, string content);
    }
    public class BroadcastProvider : IMessageProvider
    {
        public void DisplayMessage(TimeSpan duration1, string content)
        {
            Map.Broadcast(message: content, duration: (ushort)duration1.TotalSeconds);
        }
    }
    public class RueIHintProvider : IMessageProvider
    {
        public void DisplayMessage(TimeSpan timer, string content)
        {
            TimedElemRef<SetElement> elemRef = new TimedElemRef<SetElement>();
            foreach (Player player in Player.List)
            {
                SetElement element = new SetElement(Callvote.Instance.Config.HintYCoordinate, content);
                DisplayCore core = DisplayCore.Get(player.ReferenceHub);
                core.AddTemp(element, timer, elemRef);
            }
        }
    }

    public class HSMHintProvider : IMessageProvider
    {

        public void DisplayMessage(TimeSpan timer, string content)
        {
            foreach (Player player in Player.List)
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