using System;
using System.Collections.Generic;
using Callvote.Properties;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Callvote.Features
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Only public API documentation is required")]
    internal static class ServerSpecificSettings
    {
        private static SSGroupHeader settingsHeader;

        private static SSKeybindSetting yesKeybindSetting;

        private static SSKeybindSetting noKeybindSetting;

        private static SSKeybindSetting mtfKeybindSetting;

        private static SSKeybindSetting ciKeybindSetting;

        internal static IEnumerable<ServerSpecificSettingBase> CallvoteSettings { get; set; }

        internal static void RegisterSettings()
        {
            settingsHeader = new(CallvotePlugin.Instance.Config.HeaderSettingId, AssemblyInfo.Name);
            yesKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.YesKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionYes), KeyCode.Y, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionYes));
            noKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.NoKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionNo), KeyCode.U, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionNo));

            if (CallvotePlugin.Instance.Config.EnableRespawnWave)
            {
                mtfKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.MtfKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionMtf), KeyCode.I, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionMtf));
                ciKeybindSetting = new SSKeybindSetting(CallvotePlugin.Instance.Config.CiKeybindSettingId, CallvotePlugin.Instance.Translation.VoteKeybind.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionCi), KeyCode.O, hint: CallvotePlugin.Instance.Translation.KeybindHint.Replace("%Option%", CallvotePlugin.Instance.Translation.OptionCi));
            }

            CallvoteSettings =
                [
                settingsHeader,
                yesKeybindSetting,
                noKeybindSetting,
                mtfKeybindSetting,
                ciKeybindSetting
                ];

            Register(CallvoteSettings);
        }

        internal static void UnregisterSettings()
        {
            Unregister(CallvoteSettings);
        }

        private static void Register(IEnumerable<ServerSpecificSettingBase> settings)
        {
            List<ServerSpecificSettingBase> list = [.. ServerSpecificSettingsSync.DefinedSettings ?? Array.Empty<ServerSpecificSettingBase>(), .. settings];

            ServerSpecificSettingsSync.DefinedSettings = [.. list];
            ServerSpecificSettingsSync.SendToAll();
        }

        private static void Unregister(IEnumerable<ServerSpecificSettingBase> settings)
        {
            List<ServerSpecificSettingBase> list = [.. ServerSpecificSettingsSync.DefinedSettings ?? []];

            foreach (ServerSpecificSettingBase setting in settings)
            {
                list.Remove(setting);
            }

            ServerSpecificSettingsSync.DefinedSettings = list.ToArray();
            ServerSpecificSettingsSync.SendToAll();
        }
    }
}