using System.ComponentModel;

namespace Callvote
{
    public class Config
    {
        public bool Debug { get; set; } = false;
        [Description("Enable or disable Modules.")]
        public bool EnableQueue { get; set; } = true;
        public bool EnableKick { get; set; } = true;
        public bool EnableFf { get; set; } = true;
        public bool EnableKill { get; set; } = true;
        public bool EnableNuke { get; set; } = true;
        public bool EnableRespawnWave { get; set; } = true;
        public bool EnableRoundRestart { get; set; } = true;
        [Description("Changes the voting duration.")]
        public int VoteDuration { get; set; } = 30;
        [Description("Changes the maximum amount of voting each player can call in a match.")]
        public int MaxAmountOfVotesPerRound { get; set; } = 10;
        [Description("Changes the amount of time it needs to start voting after the round starts for each module.")]
        public float MaxWaitFf { get; set; } = 0;
        public float MaxWaitKill { get; set; } = 0;
        public float MaxWaitKick { get; set; } = 0;
        public float MaxWaitNuke { get; set; } = 0;
        public float MaxWaitRespawnWave { get; set; } = 0;
        public float MaxWaitRestartRound { get; set; } = 0;
        [Description("Changes the threshold to pass the voting for each module.")]
        public int ThresholdKick { get; set; } = 30;
        public int ThresholdFf { get; set; } = 30;
        public int ThresholdKill { get; set; } = 30;
        public int ThresholdNuke { get; set; } = 30;
        public int ThresholdRespawnWave { get; set; } = 30;
        public int ThresholdRestartRound { get; set; } = 30;
        [Description("Changes Callvote's broadcast size. (0 = Callvote's default size calculation algorithm)")]
        public int BroadcastSize { get; set; } = 0;
        [Description("Changes Callvote's Queue size if enabled.")]
        public int QueueSize { get; set; } = 5;
    }
}