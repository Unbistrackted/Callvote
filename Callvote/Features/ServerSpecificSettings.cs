using System;
using System.Collections.Generic;
using Callvote.Configuration;
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

        private static Translation Translation => CallvotePlugin.Instance.Translation;

        private static Config Config => CallvotePlugin.Instance.Config;

        internal static void RegisterSettings()
        {
            if (!Config.EnableSSMenu)
            {
                return;
            }

            settingsHeader = new(Config.HeaderSettingId, AssemblyInfo.Name);

            yesKeybindSetting = new SSKeybindSetting(Config.YesKeybindSettingId, Translation.VoteKeybind.Replace("%Option%", Translation.DetailYes), KeyCode.Y, hint: Translation.KeybindHint.Replace("%Option%", Translation.DetailYes));
            noKeybindSetting = new SSKeybindSetting(Config.NoKeybindSettingId, Translation.VoteKeybind.Replace("%Option%", Translation.DetailNo), KeyCode.U, hint: Translation.KeybindHint.Replace("%Option%", Translation.DetailNo));

            if (Config.EnableRespawnWave)
            {
                mtfKeybindSetting = new SSKeybindSetting(Config.MtfKeybindSettingId, Translation.VoteKeybind.Replace("%Option%", Translation.DetailMtf), KeyCode.I, hint: Translation.KeybindHint.Replace("%Option%", Translation.DetailMtf));
                ciKeybindSetting = new SSKeybindSetting(Config.CiKeybindSettingId, Translation.VoteKeybind.Replace("%Option%", Translation.DetailCi), KeyCode.O, hint: Translation.KeybindHint.Replace("%Option%", Translation.DetailCi));
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

            foreach (ServerSpecificSettingBase setting in settings ?? [])
            {
                list.Remove(setting);
            }

            ServerSpecificSettingsSync.DefinedSettings = [.. list];
            ServerSpecificSettingsSync.SendToAll();
        }
    }
}