using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Features.Pools;
using MEC;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UnityEngine;
using UserSettings.ServerSpecific;
using YamlDotNet.Serialization;

namespace Callvote.Features
{
    public static class ServerSpecificSettings
    {
        public static IEnumerable<SettingBase> CallvoteSettings { get; private set; }
        public static KeybindSetting YesKeybindSetting { get; set; }
        public static KeybindSetting NoKeybindSetting { get; set; }
        public static KeybindSetting MtfKeybindSetting { get; set; }
        public static KeybindSetting CiKeybindSetting { get; set; }
        public static void RegisterSettings()
        {
            YesKeybindSetting = new KeybindSetting(888, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionYes), KeyCode.Y, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionYes));
            NoKeybindSetting = new KeybindSetting(889, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionNo), KeyCode.U, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionNo));

            if (Callvote.Instance.Config.EnableRespawnWave)
            {
                MtfKeybindSetting = new KeybindSetting(890, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionMtf), KeyCode.I, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionMtf));
                CiKeybindSetting = new KeybindSetting(891, Callvote.Instance.Translation.VoteKeybind.Replace("%Option%", Callvote.Instance.Translation.OptionCi), KeyCode.O, hintDescription: Callvote.Instance.Translation.KeybindHint.Replace("%Option%", Callvote.Instance.Translation.OptionCi));
            }
            CallvoteSettings = SettingBase.Register(new SettingBase[] { Callvote.Instance.SettingsHeader, YesKeybindSetting, NoKeybindSetting, MtfKeybindSetting, CiKeybindSetting });
            SettingBase.SendToAll();
        }
        public static void UnregisterSettings()
        {
            SettingBase.Unregister();
        }

    }
}