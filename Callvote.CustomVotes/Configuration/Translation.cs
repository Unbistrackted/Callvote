using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callvote.CustomVotes.Configuration
{
    public class Translation
    {
        [Description("Vote Questions Messages:")]
        public string AskedToKill { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Kill %Offender% for %Reason%?";

        public string AskedToRespawn { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Respawn the next wave?";

        public string AskedToNuke { get; set; } = "%Player% <color=#EEDC8A>asks</color>: NUKE THE FACILITY?!??";

        public string AskedToDisableFf { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Disable Friendly Fire for the current round?";

        public string AskedToEnableFf { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Enable Friendly Fire for the current round?";

        public string AskedToKick { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?";

        public string AskedToRestart { get; set; } = "%Player% <color=#EEDC8A>asks</color>: Restart the round?";

        [Description("Vote Passed Messages:")]
        public string PlayerKilled { get; set; } = "%VotePercent%% voted %VoteDetail%.. Killing player %Offender%";

        public string RoundRestarting { get; set; } = "%VotePercent% voted %VoteDetail%.. Restarting the round...";

        public string FoundationNuked { get; set; } = "%VotePercent%% voted %VoteDetail%.. Nuking the facility...";

        public string CiRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. Forcing the reappearing of CI..";

        public string EnablingFriendlyFire { get; set; } = "%VotePercent%% voted %VoteDetail%. Enabling Friendly Fire..";

        public string DisablingFriendlyFire { get; set; } = "%VotePercent%% voted %VoteDetail%. Disabling Friendly Fire..";

        public string MtfRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. Forcing the reappearing of MTF..";

        public string PlayerKicked { get; set; } = "%Offender% was kicked for %Reason%.";

        [Description("Vote Failed Messages:")]
        public string NotSuccessFullKick { get; set; } = "%VotePercent%% voted %VoteDetail%, but %ThresholdKick%% was required to kick %Offender%.";

        public string NoSuccessFullRespawn { get; set; } = "%VotePercent%% voted %VoteDetail%. %ThresholdRespawnWave%% was required to respawn the next wave.";

        public string NoSuccessFullNuke { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdNuke%% was required to nuke the facility.";

        public string NoSuccessFullKill { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdKill%% was required to kill %Offender%";

        public string NoSuccessFullRestart { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdRestartRound%% was required to restart the round.";

        public string NoSuccessFullEnableFf { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdFF%% was required to disable Friendly Fire.";

        public string NoSuccessFullDisableFf { get; set; } = "Only %VotePercent%% voted %VoteDetail%. %ThresholdFF%% was required to enable Friendly Fire.";

        [Description("Commands response:")]
        public string VoteRespawnWaveDisabled { get; set; } = "Callvote RespawnWave is disabled.";

        public string VoteKickDisabled { get; set; } = "Callvote kick is disabled.";

        public string VoteKillDisabled { get; set; } = "Callvote kill is disabled.";

        public string VoteNukeDisabled { get; set; } = "Callvote nuke is disabled.";

        public string VoteFFDisabled { get; set; } = "Callvote ff is disabled.";

        public string VoteRestartRoundDisabled { get; set; } = "Callvote restartround is disabled.";

        public string WaitToVote { get; set; } = "You should wait %Timer%s before using this command.";

        public string PassReason { get; set; } = "You need to pass a reason!";

        [Description("Vote options:")]
        public string CommandChaosInsurgency { get; set; } = "ci";

        public string CommandMobileTaskForce { get; set; } = "mtf";
        public string DetailMtf { get; set; } = "<color=blue>MTF</color>";

        public string DetailCi { get; set; } = "<color=green>CI</color>";
    }
}
