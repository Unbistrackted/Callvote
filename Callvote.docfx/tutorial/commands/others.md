---
uid: othercommand
---

# Other Votings
How to call and use the ``Predefined Votings``

> [!IMPORTANT]
> Remove the square brackets when using the command, they only serve as separation in this example.

### Friendly Fire Vote

```
callvote ff
```

> [!WARNING]
> This Voting requires the ``cv.callvoteff`` permission.

> [!NOTE]
> The Voting changes based on the Friendly Fire value.

You should expect these Votings to pop up:

![alt-text](images/CallvoteEnableFF.png)

![alt-text](images/CallvoteDisableFF.png)

### Kick Player Vote

```
callvote kick [Player] [Reason]
```

> [!WARNING]
> This Voting requires the ``cv.callvotekick`` permission.

> [!NOTE]
> If the offender has the ``cv.untouchable`` permission, he won't be kicked even if the vote passes.

You should expect this Voting to pop up:

![alt-text](images/CallvoteKick.png)

### Kill Player Vote

```
callvote kill [Player] [Reason]
```

> [!WARNING]
> This Voting requires the ``cv.callvotekill`` permission.

> [!NOTE]
> If the offender has the ``cv.untouchable`` permission, he won't be killed even if the vote passes.


You should expect this Voting to pop up:

![alt-text](images/CallvoteKill.png)

### Nuke Facility Vote

```
callvote nuke
```

> [!WARNING]
> This Voting requires the ``cv.callvotenuke`` permission.

You should expect this Voting to pop up:

![alt-text](images/CallvoteNuke.png)

### Respawn Wave Vote

```
callvote respawnwave
```

> [!WARNING]
> This Voting requires the ``cv.callvoterespawnwave`` permission.

You should expect this Voting to pop up:

![alt-text](images/CallvoteRespawnWave.png)

### Restart Round Vote

```
callvote restartround
```

> [!WARNING]
> This Voting requires the ``cv.callvoterestartround`` permission.

You should expect this Voting to pop up:

![alt-text](images/CallvoteRestart.png)
