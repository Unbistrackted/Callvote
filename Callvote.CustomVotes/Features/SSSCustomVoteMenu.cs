using System.Collections.Generic;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Callvote.CustomVotes.Features
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal class SSSCustomVoteMenu
    {
        private static SSGroupHeader settingsHeader;

        private static SSKeybindSetting mtfKeybindSetting;

        private static SSKeybindSetting ciKeybindSetting;

        internal static IEnumerable<ServerSpecificSettingBase> CustomVoteSettings { get; set; }

        internal static void RegisterSettings()
        {
            if (!Plugin.Instance.Config.EnableSsKeybinds || !Plugin.Instance.Config.EnableRespawnWave)
            {
                return;
            }

            settingsHeader = new(CallvotePlugin.Instance.Config.HeaderSettingId, "Callvote Custom Votes");

            mtfKeybindSetting = new SSKeybindSetting(Plugin.Instance.Config.MtfKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", Plugin.Instance.Translation.DetailMtf), KeyCode.I, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", Plugin.Instance.Translation.DetailMtf));
            ciKeybindSetting = new SSKeybindSetting(Plugin.Instance.Config.CiKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", Plugin.Instance.Translation.DetailCi), KeyCode.O, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", Plugin.Instance.Translation.DetailCi));

            CustomVoteSettings = [settingsHeader, mtfKeybindSetting, ciKeybindSetting];

            // Thanks Zero!
            ServerSpecificSettingsSync.DefinedSettings = [.. ServerSpecificSettingsSync.DefinedSettings ?? [], .. CustomVoteSettings];
        }

        internal static void UnregisterSettings()
        {
            List<ServerSpecificSettingBase> list = [.. ServerSpecificSettingsSync.DefinedSettings ?? []];

            foreach (ServerSpecificSettingBase setting in CustomVoteSettings)
            {
                list.Remove(setting);
            }

            ServerSpecificSettingsSync.DefinedSettings = [.. list];
            ServerSpecificSettingsSync.SendToAll();
        }
    }
}
