#if EXILED
using Player = Exiled.API.Features.Player;
#else
using Player = LabApi.Features.Wrappers.Player;
#endif
using System.Linq;
using Callvote.Features.Interfaces;
using Callvote.Features.MessageProviders;
using HarmonyLib;

namespace Callvote
{
    /// <summary>
    /// Represents the type that handles Callvote's Soft Dependency System.
    /// </summary>
    public static class SoftDependency
    {
        /// <summary>
        /// Gets the <see cref="IMessageProvider"/> instance based on the configured message provider.
        /// </summary>
        public static IMessageProvider MessageProvider { get; } = GetProvider();

        private static IMessageProvider GetProvider()
        {
            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "auto")
            {
                return AutoProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "ruei")
            {
                if (TryLoadRueI(out IMessageProvider ruei))
                {
                    return ruei;
                }

                return new BroadcastProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() == "hsm")
            {
                if (TryLoadHSM(out IMessageProvider hsm))
                {
                    return hsm;
                }

                return new BroadcastProvider();
            }

            if (CallvotePlugin.Instance.Config.MessageProvider.ToLower() is "broadcast" or "bc")
            {
                return new BroadcastProvider();
            }

            return AutoProvider();
        }

        private static IMessageProvider AutoProvider()
        {
            bool isRueILoaded = TryLoadRueI(out IMessageProvider ruei);
            bool isHSMLoaded = TryLoadHSM(out IMessageProvider hsm);

            if (isHSMLoaded && isRueILoaded)
            {
                return new BroadcastProvider();
            }

            if (isRueILoaded)
            {
                return ruei;
            }

            if (isHSMLoaded)
            {
                return hsm;
            }

            return new BroadcastProvider();
        }

        private static bool TryLoadRueI(out IMessageProvider provider)
        {
            if (IsRueiPatched())
            {
                provider = new RueIHintProvider();
                return true;
            }

            provider = new BroadcastProvider();
            return false;
        }

        private static bool TryLoadHSM(out IMessageProvider provider)
        {
            // if (IsHSMPatched())
            // {
            //     provider = new HSMHintProvider();
            //     return true;
            // }
            provider = new BroadcastProvider();
            return false;
        }

        // private static bool IsHSMPatched()
        // {
        //     return Harmony.GetAllPatchedMethods().Select(Harmony.GetPatchInfo).Any(info => info?.Postfixes?.Any(p => p.owner.Contains("HintServiceMeow")) == true || info?.Prefixes?.Any(p => p.owner.Contains("HintServiceMeow")) == true);
        // }
        private static bool IsRueiPatched()
        {
            return Harmony.GetAllPatchedMethods().Select(Harmony.GetPatchInfo).Any(info => info?.Postfixes?.Any(p => p.owner.Contains("RueI")) == true || info?.Transpilers?.Any(p => p.owner.Contains("RueI")) == true);
        }
    }
}