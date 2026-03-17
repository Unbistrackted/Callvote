using System.Collections.Generic;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Callvote.Features
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal static class SSSCallvoteMenu
    {
        private static SSGroupHeader settingsHeader;

        private static SSKeybindSetting yesKeybindSetting;

        private static SSKeybindSetting noKeybindSetting;

        internal static IEnumerable<ServerSpecificSettingBase> CallvoteSettings { get; set; }

        internal static void RegisterSettings()
        {
            if (!CallvotePlugin.Instance.Config.EnableSSMenu)
            {
                return;
            }

            settingsHeader = new(CallvotePlugin.Instance.Config.HeaderSettingId, CallvotePlugin.Instance.Name);

            yesKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.YesKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.DetailYes), KeyCode.Y, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.DetailYes));
            noKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.NoKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.DetailNo), KeyCode.U, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.DetailNo));

            CallvoteSettings = [settingsHeader, yesKeybindSetting, noKeybindSetting];

            // Thanks Zero!!!!
            ServerSpecificSettingsSync.DefinedSettings = [.. ServerSpecificSettingsSync.DefinedSettings ?? [], .. CallvoteSettings];
        }

        internal static void UnregisterSettings()
        {
            List<ServerSpecificSettingBase> list = [.. ServerSpecificSettingsSync.DefinedSettings ?? []];

            foreach (ServerSpecificSettingBase setting in CallvoteSettings)
            {
                list.Remove(setting);
            }

            ServerSpecificSettingsSync.DefinedSettings = [.. list];
            ServerSpecificSettingsSync.SendToAll();
        }
    }
}