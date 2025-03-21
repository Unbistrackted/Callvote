using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Callvote
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("Enable or disable Modules.")]
        public bool EnableKick { get; set; } = true;
        public bool EnableKill { get; set; } = true;
        public bool EnableNuke { get; set; } = true;
        public bool EnableRespawnWave { get; set; } = true;
        public bool EnableRoundRestart { get; set; } = true;
        [Description("Changes the voting duration.")]
        public int VoteDuration { get; set; } = 30;
        [Description("Changes the maximum amount of voting each player can call in a match.")]
        public int MaxAmountOfVotesPerRound { get; set; } = 10;
        [Description("Changes the amount of time it needs to start voting after the round starts for each module.")]
        public float MaxWaitKill { get; set; } = 0;
        public float MaxWaitKick { get; set; } = 0;
        public float MaxWaitNuke { get; set; } = 0;
        public float MaxWaitRespawnWave { get; set; } = 0;
        public float MaxWaitRestartRound { get; set; } = 0;
        [Description("Changes the threshold to pass the voting for each module.")]
        public int ThresholdKick { get; set; } = 30;
        public int ThresholdFF { get; set; } = 30;
        public int ThresholdKill { get; set; } = 30;
        public int ThresholdNuke { get; set; } = 30;
        public int ThresholdRespawnWave { get; set; } = 30;
        public int ThresholdRestartRound { get; set; } = 30;
        [Description("Changes Callvote's broadcast size. (0 = Callvote's default size calculation algorithm)")]
        public int BroadcastSize { get; set; } = 0;
    }
}