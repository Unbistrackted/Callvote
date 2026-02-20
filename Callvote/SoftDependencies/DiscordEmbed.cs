using Callvote.SoftDependencies.DiscordEmbedProviders;
using Callvote.SoftDependencies.Interfaces;
using HarmonyLib;
using System.Linq;

namespace Callvote.SoftDependencies
{
    /// <summary>
    /// Represents the type that handles Callvote's Webhook Soft Dependency System.
    /// </summary>
    public class DiscordEmbed
    {
        /// <summary>
        /// Gets the <see cref="IWebhookProvider"/> instance based on if DiscordLab is Loaded or not.
        /// </summary>
        public static IWebhookProvider Provider => GetWebhookProvider();

        private static IWebhookProvider GetWebhookProvider()
        {
            if (CallvotePlugin.Instance.Config.DiscordChannelId == 0)
            {
                return new WebhookProvider();
            }

            if (IsDiscordLabPatchedOrLoaded())
            {
                return new DiscordLabMessageProvider();
            }

            if (IsScpDiscordLoaded())
            {
                return new ScpDiscordMessageProvider();
            }

            return new WebhookProvider();
        }

        private static bool IsDiscordLabPatchedOrLoaded()
        {
            return Harmony.GetAllPatchedMethods().Select(Harmony.GetPatchInfo).Any(info => info?.Transpilers?.Any(p => p.owner.Contains("DiscordLab.Bot")) == true) || LabApi.Loader.PluginLoader.EnabledPlugins.Any(p => p.Name.Contains("DiscordLab"));
        }

        private static bool IsScpDiscordLoaded()
        {
            return LabApi.Loader.PluginLoader.EnabledPlugins.Any(p => p.Name.Contains("SCPDiscord"));
        }
    }
}