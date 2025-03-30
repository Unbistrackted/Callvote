# Description

This plugin is the sucessor of [callvote](PatPeter/callvote), which allows calling and voting for Kick,
RestartRound, Kill,
RespawnWave, FF, or custom votes using **KEYBINDS** or **CONSOLE COMMANDS** in the same format as the Source Engine (Left 4
Dead
2/Counter-Strike: Global Offensive).

Callvote uses [RueI](https://github.com/pawslee/RueI) as a Soft Dependency, which uses Hints instead of Brodcasts. We 
highly suggest installing it since we plan on moving away from Broadcasts.

![image](https://github.com/user-attachments/assets/34b7522c-afad-4aa5-9e5b-34797eaee3c8)

![{2D20256F-794E-4164-BCF3-1AFFAFC8CF68}](https://github.com/user-attachments/assets/16dee768-58f1-45c4-b012-5eb8443e3737)

## Configuration Settings

 Setting Key                   | Value Type | Default Value | Description                                                                                    
-------------------------------|------------|---------------|------------------------------------------------------------------------------------------------
 is_enabled                    | bool       | true          | Enables or disables the plugin     
 enable_queue                  | boolean    | true          | Enables or disables Callvote's queue system
 enable_kick                   | boolean    | true          | Can players use **callvote kick**?                                                             
 enable_kill                   | boolean    | true          | Can players use **callvote kill**?     
 enable_Ff                     | boolean    | true          | Can players use **callvote ff**?  
 enable_nuke                   | boolean    | true          | Can players use **callvote nuke**?                                                             
 enable_respawn_wave           | boolean    | true          | Can players use **callvote respawnwave**?                                                      
 enable_restart_round          | boolean    | true          | Can players use **callvote restartround**?                                                     
 vote_duration                 | int        | 30            | Number of seconds for a vote to last for.                                                      
 vote_cooldown                 | int        | 5             | Cooldown (in seconds) between each callvote.                                                   
 max_amount_of_votes_per_round | float      | 10            | Maximum amount of **callvotes** a player can do in a round                                     
 max_wait_kill                 | float      | 0             | Time (in seconds) after the round starts to the command **callvote kill** be available         
 max_wait_Ff                   | float      | 0             | Time (in seconds) after the round starts to the command **callvote ff** be available    
 max_wait_kick                 | float      | 0             | Time (in seconds) after the round starts to the command **callvote kick** be available         
 max_wait_nuke                 | float      | 0             | Time (in seconds) after the round starts to the command **callvote nuke** be available         
 max_wait_respawn_wave         | float      | 0             | Time (in seconds) after the round starts to the command **callvote respawnwave** be available  
 max_wait_restart_round        | float      | 0             | Time (in seconds) after the round starts to the command **callvote restartround** be available 
 threshold_kick                | integer    | 30            | Percentage threshold required for a player to be kicked.                                       
 threshold_kill                | integer    | 30            | Percentage threshold required for a player to be killed.                                       
 threshold_nuke                | integer    | 30            | Percentage threshold required to explode ALPHA WARHEAD.      
 threshold_ff                  | integer    | 30            | Percentage threshold required for Friendly Fire be enabled or disabled
 threshold_respawn_wave        | integer    | 30            | Percentage threshold required to respawn a MTF or CI wave.                                     
 threshold_restart_round       | integer    | 30            | Percentage threshold required to restart the round.                                            
 broadcast_size                | int        | 0             | Changes the broadcast size to user's preference, leave it at 0 to use Callvote's re-size logic 
 queue_size                    | int        | 5             | Changes Callvote's Queue size if enabled. 

## Permissions

 Permission              | Command                         | Description                                                        
-------------------------|---------------------------------|--------------------------------------------------------------------
 cv.callvote             | .callvote (Parameter)           | Allows players to use **.callvote**          
 cv.managequeue          | .callvote queue (Parameter)     | Allows players to manage Callvote's queue system using **.callvote queue (Parameter)**      
 cv.bypass               | .callvote (Parameter)           | Bypasses permissions requeriments, time and maxium amount of votes 
 cv.unlimitedvotes       | .callvote (Parameter)           | Bypasses max_amount_of_votes_per_round                             
 cv.callvotekick         | .callvote kick                  | Gives permission to use **.callvote kick**                         
 cv.callvotekill         | .callvote kill                  | Gives permission to use **.callvote kill**                         
 cv.callvotenuke         | .callvote nuke                  | Gives permission to use **.callvote nuke**                         
 cv.callvoterespawnwave  | .callvote respawnwave           | Gives permission to use **.callvote respawnwave**                  
 cv.callvoterestartround | .callvote restartround          | Gives permission to use **.callvote restartround**          
 cv.callvoteff           | .callvote ff                    | Gives permission to use **.callvote ff**
 cv.callvotecustom       | .callvote "Custom Question" ... | Gives permission to use **.callvote "Custom Question" ...**        
 cv.stopvote             | .stopvote                       | Stops current vote                                                 
 cv.untouchable          | .callvote kick/kill             | Player cannot be kicked or killed                                  
 cv.superadmin+          | .callvote (Parameter)           | Allows player to rig the system :trollface:                        

## Commands

 Server Command        | Client Command         | Parameters                                           | Description                              
-----------------------|------------------------|------------------------------------------------------|------------------------------------------
 callvote              | .callvote binary       | "Custom Question"                                    | Vote on a custom yes/no question.        
 callvote              | .callvote custom       | "Custom Question" option(detail) option(detail)  ... | Vote on a question with multiple options 
 callvote kick         | .callvote kick         | [player]                                             | Call a voting to kick a player.                   
 callvote kill         | .callvote kill         | [player]                                             | Call a voting to kill a player.                   
 callvote nuke         | .callvote nuke         | [none]                                               | Call a voting to nuke the facility.               
 callvote respawnwave  | .callvote respawnwave  | [none]                                               | Call a voting to respawn a MTF or CI wave.                 
 callvote restartround | .callvote restartround | [none]                                               | Call a voting to restart a round.                 
 callvote ff           | .callvote ff           | [none]                                               | Call a voting to enable or disable Friendly Fire  
 callvote queue        | .callvote queue        | [none]                                               | List all votings in the Queue (index, votingType, player, Question)
 callvote queue check  | .callvote queue check  | [none]                                               | List all votings in the Queue (index, votingType, player, Question)
 callvote queue clear  | .callvote queue clear  | [none]                                               | Removes all votings from the Queue
 callvote queue pause  | .callvote queue pause  | [none]                                               | Pauses the Queue system
 callvote queue rp     | .callvote queue rp     | [player]                                             | Removes all votings by a Player from the Queue
 callvote queue rt     | .callvote queue rt     | [votingType]                                         | Removes all votings of votingType X from the Queue 
 callvote queue ri     | .callvote queue ri     | [index]                                              | Removes a voting with index X from the Queue
 stopvote              | .callvote stopvote     | [none]                                               | Stops a vote currently in progress       
 yes (or translation)  | .yes (or translation)  | [none]                                               | Vote option                              
 no  (or translation)  | .no  (or translation)  | [none]                                               | Vote option                              
 mtf (or translation)  | .mtf (or translation)  | [none]                                               | Vote option                              
 ci  (or translation)  | .ci  (or translation)  | [none]                                               | Vote option                              
 (custom)              | .(custom)              | [none]                                               | Vote option                              

## Config File

```
Callvote:
  is_enabled: true
  debug: false
  # Enable or disable Modules.
  enable_queue: true
  enable_kick: true
  enable_ff: true
  enable_kill: true
  enable_nuke: true
  enable_respawn_wave: true
  enable_round_restart: true
  # Changes the voting duration.
  vote_duration: 30
  # Changes the maximum amount of voting each player can call in a match.
  max_amount_of_votes_per_round: 10
  # Changes the amount of time it needs to start voting after the round starts for each module.
  max_wait_ff: 0
  max_wait_kill: 0
  max_wait_kick: 0
  max_wait_nuke: 0
  max_wait_respawn_wave: 0
  max_wait_restart_round: 0
  # Changes the threshold to pass the voting for each module.
  threshold_kick: 30
  threshold_ff: 30
  threshold_kill: 30
  threshold_nuke: 30
  threshold_respawn_wave: 30
  threshold_restart_round: 30
  # Changes Callvote's broadcast size. (0 = Callvote's default size calculation algorithm)
  broadcast_size: 0
  # Changes Callvote's Queue size if enabled.
  queue_size: 5
```

## Translation File

```
Callvote:
# %player%, %VotePercent%, %Offender%, %ThresholdKick%, %ThresholdRespawnWave%, %ThresholdNuke%, %ThresholdKill%, %ThresholdRestartRound%, %OptionKey%, %Option%, %Counter%, %Timer%, %Custom%, %Type%, %Number%
  max_vote: 'Max amounts of votes done this round'
  voting_in_progress: 'A vote is currently in progress.'
  players_with_same_name: 'Multiple players have a name or partial name of %Player%. Please use a different search string.'
  option_yes: '<color=green>YES</color>'
  option_no: '<color=red>NO</color>'
  player_kicked: '%Offender% was kicked for %Reason%.'
  asked_to_kill: '%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?'
  untouchable: '%VotePercent%% voted to kill or kick you.'
  not_success_full_kick: '%VotePercent%% voted yes, but %ThresholdKick%% was required to kick %Offender%.'
  player_not_found: 'Did not find any players with the name or partial name of %Player%'
  no_option_available: 'Vote does not have the option %Option%.'
  already_voted: 'You''ve already voted.'
  vote_accepted: 'You voted %Reason%.'
  no_permission: 'You do not have permission to run this command!'
  voting_stoped: 'Vote stopped.'
  results: |
    Final results:
  option_and_counter: ' %Option% (%Counter%) '
  options: '.%OptionKey% = %Option% '
  asked_question: |
    %Question% 
     <color=#bce3a3>Press ~ and type</color> or <color=#939383>Set the keybind in Server-specific!</color>
  option_mtf: '<color=blue>MTF</color>'
  option_ci: '<color=green>CI</color>'
  ci_respawn: '%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of CI..'
  enabling_friendly_fire: '%VotePercent%% voted <color=green>YES</color>. Disabling Friendly Fire..'
  disabling_friendly_fire: '%VotePercent%% voted <color=green>YES</color>. Enabling Friendly Fire..'
  mtf_respawn: '%VotePercent%% voted <color=green>YES</color>. Forcing the reappearing of MTF..'
  no_success_full_respawn: '%VotePercent%% voted no. %ThresholdRespawnWave%% was required to respawn the next wave.'
  asked_to_respawn: '%Player% <color=#EEDC8A>asks</color>: Respawn the next wave?'
  asked_to_nuke: '%Player% <color=#EEDC8A>asks</color>: NUKE THE FACILITY?!??'
  asked_to_disable_ff: '%Player% <color=#EEDC8A>asks</color>: Enable Friendly Fire for the current round?'
  asked_to_enable_ff: '%Player% <color=#EEDC8A>asks</color>: Disable Friendly Fire for the current round?'
  foundation_nuked: '%VotePercent%% voted yes. Nuking the facility...'
  no_success_full_nuke: 'Only %VotePercent%% voted yes. %ThresholdNuke%% was required to nuke the facility.'
  no_success_full_kill: 'Only %VotePercent%% voted yes. + %ThresholdKill%% was required to kill locatedPlayerName'
  player_killed: '%VotePercent%% voted yes. Killing player %Offender%'
  vote_respawn_wave_disabled: 'Callvote RespawnWave is disabled.'
  vote_kick_disabled: 'Callvote kick is disabled.'
  vote_kill_disabled: 'Callvote kill is disabled.'
  vote_nuke_disabled: 'Callvote nuke is disabled.'
  vote_f_f_disabled: 'Callvote ff is disabled.'
  vote_restart_round_disabled: 'Callvote restartround is disabled.'
  asked_to_kick: '%Player% <color=#EEDC8A>asks</color>: Kick %Offender% for %Reason%?'
  asked_to_restart: '%Player% <color=#EEDC8A>asks</color>: Restart the round?'
  round_restarting: '%VotePercent% voted yes. Restarting the round...'
  no_success_full_restart: 'Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to restart the round.'
  no_success_full_enable_ff: 'Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to disable Friendly Fire.'
  no_success_full_disable_ff: 'Only %VotePercent%% voted yes. %ThresholdRestartRound%% was required to enable Friendly Fire.'
  voting_started: 'Vote has been started!'
  no_voting_in_progress: 'There is no vote in progress.'
  wait_to_vote: 'You should wait %Timer%s before using this command.'
  asked_custom: '%Player% <color=#EEDC8A>asks</color>: %Custom%'
  pass_reason: 'You need to pass a reason!'
  less_than_two_options: 'You cannot create a custom voting without 2 options!'
  vote_keybind: 'Vote'
  keybind_hint: 'Set this keybind to vote'
  duplicate_command: 'It''s not possible to create a custom command with the same name!'
  queue_disabled: 'Callvote queue disabled.'
  queue_cleared: 'Votings Queue Cleared.'
  queue_paused: 'Queue Paused'
  queue_resumed: 'Votings Queue resumed.'
  removed_from_queue: 'Removed %Number% Voting(s)'
  no_voting_in_queue: 'There''s no voting in the queue.'
  type_not_found: 'Did not find any Voting with the type <color=red>%Type%</color>'
  invalid_argument: 'Invalid argument.'
  # Commands:
  command_yes: 'yes'
  command_no: 'no'
  command_chaos_insurgency: 'ci'
  command_mobile_task_force: 'mtf'
```

# Special thanks to:

https://github.com/Playeroth for helping me with new logic and translations.

https://github.com/PatPeter for giving the permission to continue the development of callvote.

https://github.com/vladflotsky for giving adivice and guidance while I was rewritting the plugin
