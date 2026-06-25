//-----------------------------------------------------------------------------
// Super Monkey Ball: Bowling Minigame Stub
//-----------------------------------------------------------------------------

function BowlingMinigame::onStart()
{
   echo("Bowling Minigame initialized!");
   // Spawn pins dynamically based on an anchor point
}

function BowlingMinigame::onEnd()
{
   echo("Bowling Minigame shutting down!");
}

function BowlingMinigame::onPlayerJoin(%client)
{
   messageClient(%client, 'MsgSystem', '\c0Welcome to Bowling! Strike it big!');
}

function BowlingMinigame::onPlayerSpawn(%player)
{
   // Lock movement to X-axis left/right shift until release, then spin
}
