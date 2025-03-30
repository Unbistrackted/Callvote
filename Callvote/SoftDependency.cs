namespace Callvote
{
    using Exiled.API.Features;
    using RueI;
    using RueI.Displays;
    using RueI.Displays.Scheduling;
    using RueI.Elements;
    using RueI.Extensions;
    using System;
    using System.Runtime.CompilerServices;

    public static class HintProvider
    {
        public static IHintProvider Provider { get; } = GetProvider();
        private static IHintProvider GetProvider()
        {
            try
            {
                LoadRueI();
                return new RueIHintProvider();
            }
            catch (Exception)
            {
                return new BroadcastProvider();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void LoadRueI() => RueIMain.EnsureInit();
    }

    public interface IHintProvider
    {
        void ShowString(TimeSpan duration, string content);
    }

    public class BroadcastProvider : IHintProvider
    {
        public void ShowString(TimeSpan duration1, string content)
        {
            Map.Broadcast(message: content, duration: (ushort)duration1.TotalSeconds);
        }
    }
    public class RueIHintProvider : IHintProvider
    {
        public void ShowString(TimeSpan timer, string content)
        {
            TimedElemRef<SetElement> elemRef = new TimedElemRef<SetElement>();
            foreach (Player player in Player.List)
            {
                SetElement element = new SetElement(300, content);
                DisplayCore core = DisplayCore.Get(player.ReferenceHub);
                core.AddTemp(element, timer, elemRef);
            }
        }
    }
}