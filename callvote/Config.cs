using Exiled.API.Interfaces;

namespace Callvote
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool EnableKick { get; set; } = true;
        public bool EnableKill { get; set; } = true;
        public bool EnableNuke { get; set; } = true;
        public bool EnableRespawnWave { get; set; } = true;
        public bool EnableRoundRestart { get; set; } = true;
        public int VoteDuration { get; set; } = 30;
        public int VoteCooldown { get; set; } = 5;
        public int MaxAmountOfVotesPerRound { get; set; } = 10;
        public float MaxWaitKill { get; set; } = 0;
        public float MaxWaitKick { get; set; } = 0;
        public float MaxWaitNuke { get; set; } = 0;
        public float MaxWaitRespawnWave { get; set; } = 0;
        public float MaxWaitRestartRound { get; set; } = 0;
        public int ThresholdKick { get; set; } = 30;
        public int ThresholdKill { get; set; } = 30;
        public int ThresholdNuke { get; set; } = 30;
        public int ThresholdRespawnWave { get; set; } = 30;
        public int ThresholdRestartRound { get; set; } = 30;
        public int BroadcastSize { get; set; } = 0;
    }
}