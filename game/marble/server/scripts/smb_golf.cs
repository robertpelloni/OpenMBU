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
   %player.setMode(2); // Restrict XYZ or similar freeze mode while aiming
   %player.client.golfState = "Aiming";
   bottomPrint(%player.client, "Golf: Aiming. Strokes: " @ %player.client.strokes, 0, 1);
}

// Custom golf command triggered to hit the ball
function serverCmdGolfSwing(%client, %power)
{
   if (%client.golfState $= "Aiming" && isObject(%client.player))
   {
      %client.golfState = "Rolling";
      %client.strokes++;

      // Calculate forward impulse based on camera direction or fixed axis
      // For simplicity, we just push forward relative to the ball's orientation
      %powerMult = ($Game::MonkeyGolf::PowerMult !$= "") ? $Game::MonkeyGolf::PowerMult : 25;
      %actualPower = %power * %powerMult;

      // Apply impulse
      %client.player.setMode(1); // Normal movement physics to roll
      %client.player.applyImpulse("0 0 0", "0" SPC %actualPower SPC "0");

      messageClient(%client, 'MsgSystem', '\c0Stroke %1! Power: %2', %client.strokes, %power);
      bottomPrint(%client, "Golf: Rolling. Strokes: " @ %client.strokes, 0, 1);

      // Schedule check to see when it stops rolling to revert to Aiming
      schedule(2000, 0, "checkGolfStop", %client);
   }
}

function checkGolfStop(%client)
{
   if (isObject(%client.player))
   {
      %vel = %client.player.getVelocity();
      %speed = VectorLen(%vel);

      if (%speed < 0.1)
      {
         // Ball stopped
         %client.golfState = "Aiming";
         %client.player.setMode(2); // Freeze again
         bottomPrint(%client, "Golf: Aiming. Strokes: " @ %client.strokes, 0, 1);
      }
      else
      {
         // Still moving, check again soon
         schedule(500, 0, "checkGolfStop", %client);
      }
   }
}
