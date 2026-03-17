
using System.ComponentModel;

namespace Callvote.CustomVotes.Configuration
{
    public class Config
    {
        [Description("SSS Menu related:")]
        public bool EnableSsKeybinds { get; set; } = true;

        public int MtfKeybindSettingId { get; set; } = 890;

        public int CiKeybindSettingId { get; set; } = 891;

        [Description("Enables or disables the vote for each module.")]
        public bool EnableKick { get; set; } = true;

        public bool EnableFf { get; set; } = true;

        public bool EnableKill { get; set; } = true;

        public bool EnableNuke { get; set; } = true;

        public bool EnableRespawnWave { get; set; } = true;

        public bool EnableRoundRestart { get; set; } = true;

        [Description("Changes the amount of time it needs to start vote after the round starts for each module.")]
        public float MaxWaitFf { get; set; } = 0;

        public float MaxWaitKill { get; set; } = 0;

        public float MaxWaitKick { get; set; } = 0;

        public float MaxWaitNuke { get; set; } = 0;

        public float MaxWaitRespawnWave { get; set; } = 0;

        public float MaxWaitRestartRound { get; set; } = 0;

        [Description("Changes the threshold to pass the vote for each module.")]
        public int ThresholdKick { get; set; } = 30;

        public int ThresholdFf { get; set; } = 30;

        public int ThresholdKill { get; set; } = 30;

        public int ThresholdNuke { get; set; } = 30;

        public int ThresholdRespawnWave { get; set; } = 30;

        public int ThresholdRestartRound { get; set; } = 30;

    }
}
