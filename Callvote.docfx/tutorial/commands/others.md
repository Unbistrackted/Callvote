---
uid: othercommand
---

# Other Votes
How to call and use the ``Predefined Votes``

> [!IMPORTANT]
> Remove the square brackets when using the command, they only serve as separation in this example.

### Friendly Fire Vote

```
callvote ff
```

> [!WARNING]
> This Vote requires the ``cv.callvoteff`` permission.

> [!NOTE]
> The Vote changes based on the Friendly Fire value.

You should expect these Vote to pop up:

![alt-text](~/images/CallvoteEnableFF.png)

![alt-text](~/images/CallvoteDisableFF.png)

### Kick Player Vote

```
callvote kick [Player] [Reason]
```

> [!WARNING]
> This Vote requires the ``cv.callvotekick`` permission.

> [!NOTE]
> If the offender has the ``cv.untouchable`` permission, he won't be kicked even if the vote passes.

You should expect this Vote to pop up:

![alt-text](~/images/CallvoteKick.png)

### Kill Player Vote

```
callvote kill [Player] [Reason]
```

> [!WARNING]
> This Vote requires the ``cv.callvotekill`` permission.

> [!NOTE]
> If the offender has the ``cv.untouchable`` permission, he won't be killed even if the vote passes.


You should expect this Vote to pop up:

![alt-text](~/images/CallvoteKill.png)

### Nuke Facility Vote

```
callvote nuke
```

> [!WARNING]
> This Vote requires the ``cv.callvotenuke`` permission.

You should expect this Vote to pop up:

![alt-text](~/images/CallvoteNuke.png)

### Respawn Wave Vote

```
callvote respawnwave
```

> [!WARNING]
> This Vote requires the ``cv.callvoterespawnwave`` permission.

You should expect this Vote to pop up:

![alt-text](~/images/CallvoteRespawnWave.png)

### Restart Round Vote

```
callvote restartround
```

> [!WARNING]
> This Vote requires the ``cv.callvoterestartround`` permission.

You should expect this Vote to pop up:

![alt-text](~/images/CallvoteRestart.png)
