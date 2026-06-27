//-----------------------------------------------------------------------------
// Super Monkey Ball: Golf Minigame Stub
//-----------------------------------------------------------------------------

function GolfMinigame::onStart()
{
   echo("Golf Minigame initialized!");
   // Handle hole placement, tee off order, stroke counting, etc.
}

function GolfMinigame::onEnd()
{
   echo("Golf Minigame shutting down!");
}

function GolfMinigame::onPlayerJoin(%client)
{
   %client.strokes = 0;
   messageClient(%client, 'MsgSystem', '\c0Welcome to Monkey Golf! Lowest strokes wins.');
}

function GolfMinigame::onPlayerSpawn(%player)
{
   // Turn off standard MoveMap WASD input, rely instead on a swing power meter
}
