using Exiled.API.Features.Core.UserSettings;
using UnityEngine;

namespace Callvote.Features
{
    public static class ServerSpecificSettings
    {
        public static KeybindSetting YesKeybindSetting { get; set; }
        public static KeybindSetting NoKeybindSetting { get; set; }
        public static KeybindSetting MtfKeybindSetting { get; set; }
        public static KeybindSetting CiKeybindSetting { get; set; }
        public static void RegisterSettings()
        {
            YesKeybindSetting = new KeybindSetting(888, Callvote.Instance.Translation.VoteKeybind.Replace("%Option", Callvote.Instance.Translation.OptionYes), KeyCode.Y, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option", Callvote.Instance.Translation.OptionYes));
            SettingBase.Register(new[] { YesKeybindSetting });
            NoKeybindSetting = new KeybindSetting(889, Callvote.Instance.Translation.VoteKeybind.Replace("%Option", Callvote.Instance.Translation.OptionNo), KeyCode.U, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option", Callvote.Instance.Translation.OptionNo));
            SettingBase.Register(new[] { NoKeybindSetting });

            if (Callvote.Instance.Config.EnableRespawnWave)
            {
                MtfKeybindSetting = new KeybindSetting(890, Callvote.Instance.Translation.VoteKeybind.Replace("%Option", Callvote.Instance.Translation.OptionMtf), KeyCode.I, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option", Callvote.Instance.Translation.OptionMtf));
                SettingBase.Register(new[] { MtfKeybindSetting });

                CiKeybindSetting = new KeybindSetting(891, Callvote.Instance.Translation.VoteKeybind.Replace("%Option", Callvote.Instance.Translation.OptionCi), KeyCode.O, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option", Callvote.Instance.Translation.OptionCi));
                SettingBase.Register(new[] { CiKeybindSetting });
            }
        }
        public static void UnregisterSettings()
        {
            if (Callvote.Instance.Config.EnableRespawnWave)
            {
                SettingBase.Unregister(settings: new[] { MtfKeybindSetting });
                SettingBase.Unregister(settings: new[] { CiKeybindSetting });
            }
            SettingBase.Unregister(settings: new[] { YesKeybindSetting });
            SettingBase.Unregister(settings: new[] { NoKeybindSetting });
        }
    }
}