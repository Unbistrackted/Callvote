using Callvote.SoftDependencies.Interfaces;
using Callvote.SoftDependencies.MessageProviders;
using HarmonyLib;
using System.Linq;

namespace Callvote.SoftDependencies
{
    /// <summary>
    /// Represents the type that handles Callvote's Message Soft Dependency System.
    /// </summary>
    internal class DisplayMessage
    {
        /// <summary>
        /// Gets the <see cref="IMessageProvider"/> instance based on the configured message provider.
        /// </summary>
        public static IMessageProvider Provider => GetMessageProvider();

        private static IMessageProvider GetMessageProvider()
        {
            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "auto")
            {
                return AutoMessageProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "ruei")
            {
                if (IsRueiPatched())
                {
                    return new RueIHintProvider();
                }

                return new BroadcastProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "hsm")
            {
                if (IsHSMPatched())
                {
                    return new HSMHintProvider();
                }

                return new BroadcastProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() is "broadcast" or "bc")
            {
                return new BroadcastProvider();
            }

            return AutoMessageProvider();
        }

        private static IMessageProvider AutoMessageProvider()
        {
            bool isRueILoaded = IsRueiPatched();
            bool isHSMLoaded = IsHSMPatched();

            if (isHSMLoaded && isRueILoaded)
            {
                return new BroadcastProvider();
            }

            if (isRueILoaded)
            {
                return new RueIHintProvider();
            }

            if (isHSMLoaded)
            {
                return new HSMHintProvider();
            }

            return new BroadcastProvider();
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
}
