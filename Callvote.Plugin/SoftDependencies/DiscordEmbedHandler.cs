using System.Linq;
using Callvote.SoftDependencies.DiscordEmbed;
using Callvote.SoftDependencies.DiscordEmbedProviders;
using HarmonyLib;

namespace Callvote.SoftDependencies
{
    /// <summary>
    /// Represents the type that handles Callvote's Webhook Soft Dependency System.
    /// </summary>
    public class DiscordEmbedHandler
    {
        /// <summary>
        /// Gets the current instance of the embed provider used for handling webhooks.
        /// </summary>
        public static EmbedProvider CurrentProvider => GetWebhookProvider();

        /// <summary>
        /// Gets the webhook provider to use for sending vote results and vote started messages to Discord, based on the plugins that are currently loaded and patched in the server.
        /// </summary>
        /// <returns>Gets the webhook provider to use.</returns>
        private static EmbedProvider GetWebhookProvider()
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