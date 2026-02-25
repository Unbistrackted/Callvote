using System.Linq;
using Callvote.API.Providers.SendDiscordEmbed;
using Callvote.SoftDependencies.DiscordEmbedProviders;
using HarmonyLib;

namespace Callvote.SoftDependencies
{
    /// <summary>
    /// Represents the type that handles Callvote's Webhook Soft Dependency System.
    /// </summary>
    public class DiscordEmbed
    {
        public static EmbedProvider GetWebhookProvider()
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