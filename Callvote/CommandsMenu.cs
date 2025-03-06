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
            YesKeybindSetting = new KeybindSetting(id: 888, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionYes}!", UnityEngine.KeyCode.Y, hintDescription: Plugin.Instance.Translation.KeybindHint);
            SettingBase.Register(new[] { YesKeybindSetting });
            NoKeybindSetting = new KeybindSetting(id: 889, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionNo}!", UnityEngine.KeyCode.U, hintDescription: Plugin.Instance.Translation.KeybindHint);
            SettingBase.Register(new[] { NoKeybindSetting });

            if (Plugin.Instance.Config.EnableRespawnWave)
            {
                MtfKeybindSetting = new KeybindSetting(id: 890, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionMtf}!", UnityEngine.KeyCode.I, hintDescription: Plugin.Instance.Translation.KeybindHint);
                SettingBase.Register(new[] { MtfKeybindSetting });

                CiKeybindSetting = new KeybindSetting(id: 891, label: $"{Plugin.Instance.Translation.VoteKeybind} {Plugin.Instance.Translation.OptionCi}!", UnityEngine.KeyCode.O, hintDescription: Plugin.Instance.Translation.KeybindHint);
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
