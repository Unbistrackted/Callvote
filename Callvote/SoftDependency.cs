namespace Callvote
{
    using LabApi.Features.Wrappers;
    using RueI;
    using RueI.Displays;
    using RueI.Displays.Scheduling;
    using RueI.Elements;
    using RueI.Extensions;
    using System;
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

            if (IsRueILoaded)
            {
                return ruei;
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
            Server.SendBroadcast(message: content, duration: (ushort)duration1.TotalSeconds);
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
}