# Callvote ![Version](https://img.shields.io/github/v/release/Unbistrackted/Callvote?style=plastic&label=Version&color=dc3e3e) ![Downloads](https://img.shields.io/github/downloads/Unbistrackted/Callvote/total?style=plastic&label=Downloads&color=50f63f) 

**The sucessor of [callvote](https://github.com/PatPeter/callvote).**

A plugin that allows calling and voting for **Kick**,
 **RestartRound**,  **Kill**,
**RespawnWave**, **FriendlyFire**, or **Custom** Votes using **KEYBINDS** or **CONSOLE COMMANDS** in the same format as the Source Engine (Left 4
Dead
2/Counter-Strike: Global Offensive).

![{99C91142-43DC-4685-A9DC-C906CEC4C5CC}](https://github.com/user-attachments/assets/f74318d3-f066-4abb-b24a-3cb0187f6dcf)

If you want to develop using Callvote, please install the [Nuget Package](https://www.nuget.org/packages/Callvote)

## Examples
```cs
VotingHandler.AddOptionToVoting("nothing", "<size=100>NOTHING!!!</color>");
VotingHandler.AddOptionToVoting("nothing1", "<size=100>NOTHING1!!!</color>");
VotingHandler.CallVoting(new CustomVoting(player, $"{player.Nickname} asks: Do nothing!!!!!", "NothingBurguerPlugin.DoNothing"));
```
```cs
VotingHandler.CallVoting(new CustomVoting(player, $"{player.Nickname} asks: Enable FF?", "NothingBurguerPlugin.FF", new FFVoting(player)));
```
```cs
Dictionary<string, string> options = new()
{
   ["nothing"] = "<size=100>NOTHING!!!</color>",
   ["nothing1"] = "<size=100>NOTHING2!!!</color>"
};

CustomVoting vote = new CustomVoting(player, $"{player.Nickname} asks: Do nothing!!!!!", "NothingBurguerPlugin.DoNothing", null, options);

VotingHandler.CallVoting(vote);
```
```cs

private void ReviveSCPs(DiedEventArgs ev)
{
   if (ev.Player.IsScp)
      {
         void callback(Voting vote)
         {
            int yes = vote.Counter[Callvote.Callvote.Instance.Translation.CommandYes];
            int no = vote.Counter[Callvote.Callvote.Instance.Translation.CommandNo];
            if (yes > no)
            {
               ev.Player.RoleManager.ServerSetRole(ev.TargetOldRole, PlayerRoles.RoleChangeReason.None);
               Map.Broadcast(5, $"{ev.TargetOldRole} respawned.");
               return;
            }
                    Map.Broadcast(5, "The Voting Failed.");
         }
         BinaryVoting reviveSCP = new BinaryVoting(Server.Host, $"Revive {ev.TargetOldRole}?", $"NothingBurguerPlugin.Respawn", callback);
         VotingHandler.CallVoting(reviveSCP);
      }
}
```

## Configuration Settings:

 Setting Key                   | Value Type | Default Value | Description                                                                                    
-------------------------------|------------|---------------|------------------------------------------------------------------------------------------------
 is_enabled                    | bool       | true          | Enables or disables the plugin     
 message_provider              | string     | Auto          | Which message provider should Callvote use? You can choose between auto, hsm, ruei, or broadcasts / bc. (In auto mode, if both HSM and RUEI are present on the server, it falls back to broadcasts.)    
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
 refresh_interval              | float      | 1             | Changes message's refresh time.
 final_results_duration        | int        | 5             | Changes Callvote's results message duration.
 queue_size                    | int        | 5             | Changes Callvote's Queue size if enabled. 
 discord_webhook               | string     |               | If a Discord Webhook is present, send a message via that Webhook

## Permissions:

 Permission              | Command                         | Description                                                        
-------------------------|---------------------------------|--------------------------------------------------------------------
 cv.callvote             | .callvote (Parameter)           | Allows players to use **.callvote**          
 cv.managequeue          | .callvote queue (Parameter)     | Allows players to manage Callvote's queue system using **.callvote queue (Parameter)**      
 cv.bypass               | .callvote (Parameter)           | Bypasses time requeriments and maxium amount of votes 
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
 cv.superadmin+          | .callvote rig                   | Allows player to rig the system :trollface:                      
 cv.translation          | .callvote translation           | Allows player to change Callvote's translation                      

## Commands:

 Command               | Required Permission    | Parameters                                           | Description                              
-----------------------|------------------------|------------------------------------------------------|------------------------------------------
 callvote binary       | cv.callvotecustom      | [Custom Question]                                    | Vote on a custom yes/no question.        
 callvote custom       | cv.callvotecustom      | ["Custom Question"] [option1(detail)] [option2(detail)] ... | Vote on a question with multiple options (If question has spaces, it needs to be inside quotes)
 callvote kick         | cv.callvotekick        | [player]                                             | Call a voting to kick a player.                   
 callvote kill         | cv.callvotekill        | [player]                                             | Call a voting to kill a player.                   
 callvote nuke         | cv.callvotenuke        | [none]                                               | Call a voting to nuke the facility.               
 callvote respawnwave  | cv.callvoterespawnwave | [none]                                               | Call a voting to respawn a MTF or CI wave.                 
 callvote restartround | cv.callvoterestartround| [none]                                               | Call a voting to restart a round.                 
 callvote ff           | cv.callvoteff          | [none]                                               | Call a voting to enable or disable Friendly Fire  
 callvote queue        | cv.managequeue         | [none]                                               | List all votings in the Queue (index, votingType, player, Question)
 callvote queue check  | cv.managequeue         | [none]                                               | List all votings in the Queue (index, votingType, player, Question)
 callvote queue clear  | cv.managequeue         | [none]                                               | Removes all votings from the Queue
 callvote queue pause  | cv.managequeue         | [none]                                               | Pauses the Queue system
 callvote queue rp     | cv.managequeue         | [player]                                             | Removes all votings by a Player from the Queue
 callvote queue rt     | cv.managequeue         | [votingType]                                         | Removes all votings of votingType X from the Queue 
 callvote queue ri     | cv.managequeue         | [index]                                              | Removes a voting with index X from the Queue
 callvote translation  | cv.managequeue         | [none/language/countryCode]                          | Changes Callvote's translation (If nothing is passed as an argument or countryCoude/language is wrong, tries to find server location.)
 stopvote              | cv.stopvote            | [none]                                               | Stops a vote currently in progress       
 .yes (or translation) |                        | [none]                                               | Vote option                              
 .no  (or translation) |                        | [none]                                               | Vote option                              
 .mtf (or translation) |                        | [none]                                               | Vote option                              
 .ci  (or translation) |                        | [none]                                               | Vote option                              
 .(custom)             |                        | [none]                                               | Vote option                              

## Download

This plugin requires [Exiled](https://github.com/ExMod-Team/EXILED/releases/latest) or [LabAPI](https://github.com/northwood-studios/LabAPI/releases/latest).

You can download the latest version of Callvote [here](https://github.com/Unbistrackted/Callvote/releases/latest).

## Soft Depedencies

### Replaces Broadcasts with Hints.
❗️ Do **not** install both — they are incompatible with each other.

- [RueI](https://github.com/pawslee/RueI) — You can download it [here](https://github.com/pawslee/RueI/releases/latest).

- [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow/releases/tag/V5.3) — You can download it [here](https://github.com/MeowServer/HintServiceMeow/releases/latest). 

## Special thanks to:

https://github.com/Playeroth and https://github.com/Edi369 for helping me with translations and adding webhook functionality.

https://github.com/PatPeter for giving the permission to continue the development of [callvote](https://github.com/PatPeter/callvote).

https://github.com/vladflotsky for giving adivice and guidance while I was rewritting the plugin.
