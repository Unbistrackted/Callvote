using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Callvote
{
    public class Translation : ITranslation
    {
        [Description("%player%, %VotePercent%, %Offender%, %ThresholdKick%, %ThresholdRespawnWave%, %ThresholdNuke%, %ThresholdKill%, %ThresholdRestartRound%, %OptionKey%, %Option%, %Counter%, %Timer%, %Custom%, %Type%, %Number%")]
        public string MaxVote { get; private set; } = "Max amounts of votes done this round";
        public string VotingInProgress { get; private set; } = "A vote is currently in progress.";
        public string PlayersWithSameName { get; private set; } = "Multiple players have a name or partial name of %Player%. Please use a different search string.";
        public string OptionYes { get; private set; } = "<color=green>YES</color>";
        public string OptionNo { get; private set; } = "<color=red>NO</color>";
        public string PlayerKicked { get; private set; } = "%Offender% was kicked for %Reason%.";
        public string AskedToKill { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?";
        public string Untouchable { get; private set; } = "%VotePercent%% voted to kill or kick you.";
        public string NotSuccessFullKick { get; private set; } = "%VotePercent%% voted yes, but %ThresholdKick%% was required to kick %Offender%.";
        public string PlayerNotFound { get; private set; } = "Did not find any players with the name or partial name of %Player%";
        public string NoOptionAvailable { get; private set; } = "Vote does not have the option %Option%.";
        public string AlreadyVoted { get; private set; } = "You've already voted.";
        public string VoteAccepted { get; private set; } = "You voted %Reason%.";
        public string NoPermission { get; private set; } = "You do not have permission to run this command!";
        public string VotingStoped { get; private set; } = "Vote stopped.";
        public string Results { get; private set; } = "Final results:\n";
        public string OptionAndCounter { get; private set; } = " %Option% (%Counter%) ";
        public string Options { get; private set; } = ".%OptionKey% = %Option% ";
        public string AskedQuestion { get; private set; } = "%Question% \n <color=#bce3a3>Press ~ and type</color> or <color=#939383>Set the keybind in Server-specific!</color>\n";
        public string OptionMtf { get; private set; } = "<color=blue>MTF</color>";
        public string OptionCi { get; private set; } = "<color=green>CI</color>";
        public string CiRespawn { get; private set; } = "%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of CI..";
        public string EnablingFriendlyFire { get; private set; } = "%VotePercent%% voted <color=green>YES</color>. Disabling Friendly Fire..";
        public string DisablingFriendlyFire { get; private set; } = "%VotePercent%% voted <color=green>YES</color>. Enabling Friendly Fire..";
        public string MtfRespawn { get; private set; } = "%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of MTF..";
        public string NoSuccessFullRespawn { get; private set; } = "%VotePercent%% voted no. %ThresholdRespawnWave%% was required to respawn the next wave.";
        public string AskedToRespawn { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Respawn the next wave?";
        public string AskedToNuke { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: NUKE THE FACILITY?!??";
        public string AskedToDisableFf { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Enable Friendly Fire for the current round?";
        public string AskedToEnableFf { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Disable Friendly Fire for the current round?";
        public string FoundationNuked { get; private set; } = "%VotePercent%% voted yes. Nuking the facility...";
        public string NoSuccessFullNuke { get; private set; } = "Only %VotePercent%% voted yes. %ThresholdNuke%% was required to nuke the facility.";
        public string NoSuccessFullKill { get; private set; } = "Only %VotePercent%% voted yes. + %ThresholdKill%% was required to kill locatedPlayerName";
        public string PlayerKilled { get; private set; } = "%VotePercent%% voted yes. Killing player %Offender%";
        public string VoteRespawnWaveDisabled { get; private set; } = "Callvote RespawnWave is disabled.";
        public string VoteKickDisabled { get; private set; } = "Callvote kick is disabled.";
        public string VoteKillDisabled { get; private set; } = "Callvote kill is disabled.";
        public string VoteNukeDisabled { get; private set; } = "Callvote nuke is disabled.";
        public string VoteFFDisabled { get; private set; } = "Callvote ff is disabled.";
        public string VoteRestartRoundDisabled { get; private set; } = "Callvote restartround is disabled.";
        public string AskedToKick { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?";
        public string AskedToRestart { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: Restart the round?";
        public string RoundRestarting { get; private set; } = "%VotePercent% voted yes. Restarting the round...";
        public string NoSuccessFullRestart { get; private set; } = "Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to restart the round.";
        public string NoSuccessFullEnableFf { get; private set; } = "Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to disable Friendly Fire.";
        public string NoSuccessFullDisableFf { get; private set; } = "Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to enable Friendly Fire.";
        public string VotingStarted { get; private set; } = "Vote has been started!";
        public string NoVotingInProgress { get; private set; } = "There is no vote in progress.";
        public string WaitToVote { get; private set; } = "You should wait %Timer%s before using this command.";
        public string AskedCustom { get; private set; } = "%Player% <color=#EEDC8A>asks</color>: %Custom%";
        public string PassReason { get; private set; } = "You need to pass a reason!";
        public string LessThanTwoOptions { get; private set; } = "You cannot create a custom voting without 2 options!";
        public string VoteKeybind { get; private set; } = "Vote";
        public string KeybindHint { get; private set; } = "Set this keybind to vote";
        public string DuplicateCommand { get; private set; } = "It's not possible to create a custom command with the same name!";
        public string QueueDisabled { get; private set; } = "Callvote queue disabled.";
        public string QueueCleared { get; private set; } = "Votings Queue Cleared.";
        public string QueuePaused { get; private set; } = "Queue Paused";
        public string QueueResumed { get; private set; } = "Votings Queue resumed.";
        public string RemovedFromQueue { get; private set; } = "Removed %Number% Voting(s)";
        public string NoVotingInQueue { get; private set; } = "There's no voting in the queue.";
        public string TypeNotFound { get; private set; } = "Did not find any Voting with the type <color=red>%Type%</color>";
        public string InvalidArgument { get; private set; } = "Invalid argument.";
        [Description("Commands:")]
        public string CommandYes { get; set; } = "yes";
        public string CommandNo { get; set; } = "no";
        public string CommandChaosInsurgency { get; set; } = "ci";
        public string CommandMobileTaskForce { get; set; } = "mtf";
    }
}