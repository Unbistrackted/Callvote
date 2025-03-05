using Exiled.API.Features.Core.UserSettings;
using System.Collections.Generic;
using System.ComponentModel;
using UserSettings.ServerSpecific;

namespace Callvote
{
    public static class ServerSpecificSettings
    {
        public static KeybindSetting YesKeybindSetting { get; set; }
        public static KeybindSetting NoKeybindSetting { get; set; }
        public static KeybindSetting MtfKeybindSetting { get; set; }
        public static KeybindSetting CiKeybindSetting { get; set; }
        public static void RegisterSettings()
        {
            YesKeybindSetting = new KeybindSetting(id: 645, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionYes}!", default, hintDescription: Plugin.Instance.Translation.KeybindHint);
            SettingBase.Register(new[] { YesKeybindSetting });
            NoKeybindSetting = new KeybindSetting(id: 646, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionNo}!", default, hintDescription: Plugin.Instance.Translation.KeybindHint);
            SettingBase.Register(new[] { NoKeybindSetting });

            if (Plugin.Instance.Config.EnableRespawnWave)
            {
                MtfKeybindSetting = new KeybindSetting(id: 643, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionMtf}!", default, hintDescription: Plugin.Instance.Translation.KeybindHint);
                SettingBase.Register(new[] { MtfKeybindSetting });

                CiKeybindSetting = new KeybindSetting(id: 644, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionCi}!", default, hintDescription: Plugin.Instance.Translation.KeybindHint);
                SettingBase.Register(new[] { CiKeybindSetting });
            }
        }
        public static void UnregisterSettings()
        {
            if (Plugin.Instance.Config.EnableRespawnWave)
            {
                SettingBase.Unregister(settings: new[] { MtfKeybindSetting });
                SettingBase.Unregister(settings: new[] { CiKeybindSetting });
            }
            SettingBase.Unregister(settings: new[] { YesKeybindSetting });
            SettingBase.Unregister(settings: new[] { NoKeybindSetting });
        }
    }
}
