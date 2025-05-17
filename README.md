# Callvote ![Downloads](https://img.shields.io/github/downloads/Unbistrackted/Callvote/total)

**The sucessor of [callvote](https://github.com/PatPeter/callvote).**

A plugin that allows calling and voting for **Kick**,
 **RestartRound**,  **Kill**,
**RespawnWave**, **FriendlyFire**, or **Custom** Votes using **KEYBINDS** or **CONSOLE COMMANDS** in the same format as the Source Engine (Left 4
Dead
2/Counter-Strike: Global Offensive).

![{99C91142-43DC-4685-A9DC-C906CEC4C5CC}](https://github.com/user-attachments/assets/f74318d3-f066-4abb-b24a-3cb0187f6dcf)

## Configuration Settings:

 Setting Key                   | Value Type | Default Value | Description                                                                                    
-------------------------------|------------|---------------|------------------------------------------------------------------------------------------------
 is_enabled                    | bool       | true          | Enables or disables the plugin     
 message_provider              | string     | Auto          | Which message provider should Callvote use? You can choose between auto, hsm, ruei, or broadcasts. (In auto mode, if both HSM and RUEI are present on the server, it falls back to broadcasts.)    
 hint_y_coordinate             | float      | 300           | Sets the Y coordinate of the hint on a scale from 0-1000, where 0 represents the bottom of the screen (Doesn't apply for broadcasts)  
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
 discord_webhook               | string     |               | If a Discord Webhook is present, send a message via that Webhook

## Permissions:

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
 cv.translation          | .callvote (Parameter)           | Allows player to change Callvote's translation                      

## Commands:

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
 callvote translation  | .callvote translation  | [none/countryCode] - RU, CN, PT, BR, EN, KZ, BY, UA  | Changes Callvote's translation (If nothing is passed as an argument, tries to find server location.)
 stopvote              | .callvote stopvote     | [none]                                               | Stops a vote currently in progress       
 yes (or translation)  | .yes (or translation)  | [none]                                               | Vote option                              
 no  (or translation)  | .no  (or translation)  | [none]                                               | Vote option                              
 mtf (or translation)  | .mtf (or translation)  | [none]                                               | Vote option                              
 ci  (or translation)  | .ci  (or translation)  | [none]                                               | Vote option                              
 (custom)              | .(custom)              | [none]                                               | Vote option                              

## Download

This plugin requires [Exiled](https://github.com/ExMod-Team/EXILED/releases/tag/v9.5.1).

You can download the latest version of Callvote [here](https://github.com/Unbistrackted/Callvote/releases/latest).

~~Or you can type ```hub install Callvote``` in your console.~~ (As of 5/17/2025, Exiled's hub is disabled.)

## Soft Depedencies

### RueI is the most recommended. Both replaces the majority of Broadcasts with Hints

[RueI](https://github.com/pawslee/RueI) ~ You can download it [here](https://github.com/pawslee/RueI/releases/latest).

[HintServiceMeow](https://github.com/MeowServer/HintServiceMeow/releases/tag/V5.3) ~ You can download it [here](https://github.com/MeowServer/HintServiceMeow/releases/tag/V5.3). 

## Special thanks to:

https://github.com/Playeroth and https://github.com/Edi369 for helping me with translations and adding webhook functionality.

https://github.com/PatPeter for giving the permission to continue the development of [callvote](PatPeter/callvote).

https://github.com/vladflotsky for giving adivice and guidance while I was rewritting the plugin.
