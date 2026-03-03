using System.Linq;
using Callvote.API.Features.Displays;
using Callvote.API.Features.Displays.DefaultProviders;
using Callvote.SoftDependencies.MessageProviders;
using HarmonyLib;

namespace Callvote.SoftDependencies
{
    /// <summary>
    /// Represents the type that handles Callvote's Message Soft Dependency System.
    /// </summary>
    internal class DisplayMessageHandler
    {
        /// <summary>
        /// Gets the <see cref="DisplayProvider"/> instance based on the configured message provider.
        /// </summary>
        /// <returns>The <see cref="DisplayProvider"/> that is going to be used.</returns>
        public static DisplayProvider GetMessageProvider()
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

        private static DisplayProvider AutoMessageProvider()
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
