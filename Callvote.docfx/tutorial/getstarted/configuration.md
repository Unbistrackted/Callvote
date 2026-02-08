---
uid: config
---

# Configuration
How to customize and work with your ``Callvote Instance``

Callvote allows fully customization over the ``Votings``, and it shouldn't be a suprise that  we also allow configuring the ``Voting Time`` and changing their ``Texts``.

Anything related to ``Callvote's`` Behaviour is located at the Config file:

# [<span style="color:#43CCE5">LabAPI</span>](#tab/LabAPI)
### Config File Location

> [!WINDOWS]
> %AppData%\SCP Secret Laboratory\LabAPI\configs\\``{Server_Port}``\Callvote\config.yml

> [!LINUX]
> ~/.config/SCP Secret Laboratory/LabAPI/plugins/configs/``{Server_Port}``/Callvote/config.yml

# [<span style="color:#F53B3B">EXILED</span>](#tab/Exiled)
### Config File Location

> [!WINDOWS]
> %AppData%\EXILED\Configs\Plugins\Callvote\\``{Server_Port}``.yml

> [!LINUX]
> ~/.config/EXILED/Configs/Plugins/Callvote/``{Server_Port}``.yml

---

### Configuration Keys
Description and ``Type`` for each ``Setting`` in the config file.

 ``Setting Key``               | ``Value Type`` | ``Default Value`` | ``Description``
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
