//-----------------------------------------------------------------------------
// Super Monkey Ball: Golf Minigame Stub
//-----------------------------------------------------------------------------

function GolfMinigame::onStart()
{
   echo("Golf Minigame initialized!");
   $Game::MonkeyGolfActive = true;
   // Handle hole placement, tee off order, stroke counting, etc.
}

function GolfMinigame::onEnd()
{
   echo("Golf Minigame shutting down!");
   $Game::MonkeyGolfActive = false;
}

function GolfMinigame::onPlayerJoin(%client)
{
   PartyGame::initClientScore(%client, '\c0Welcome to Monkey Golf! Lowest strokes wins.');
}

function GolfMinigame::onPlayerSpawn(%player)
{
   // Turn off standard MoveMap WASD input, rely instead on a swing power meter
   %player.setMode(2); // Restrict XYZ or similar freeze mode while aiming
   %player.client.golfState = "Aiming";
   bottomPrint(%player.client, "Golf: Aiming. Strokes: " @ %player.client.score, 0, 1);
}

// Custom golf command triggered to hit the ball
function serverCmdGolfSwing(%client, %power)
{
   if (%client.golfState $= "Aiming" && isObject(%client.player))
   {
      %client.golfState = "Rolling";
      %client.score++;

      // Calculate forward impulse based on camera direction or fixed axis
      // For simplicity, we just push forward relative to the ball's orientation
      %powerMult = ($Game::MonkeyGolf::PowerMult !$= "") ? $Game::MonkeyGolf::PowerMult : 25;
      %actualPower = %power * %powerMult;

      // Apply impulse
      %forwardVec = %client.player.getForwardVector();
      %impulseVec = VectorScale(%forwardVec, %actualPower);

      %client.player.setMode(1); // Normal movement physics to roll
      %client.player.applyImpulse("0 0 0", %impulseVec);

      messageClient(%client, 'MsgSystem', '\c0Stroke %1! Power: %2', %client.score, %power);
      bottomPrint(%client, "Golf: Rolling. Strokes: " @ %client.score, 0, 1);

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
         bottomPrint(%client, "Golf: Aiming. Strokes: " @ %client.score, 0, 1);
      }
      else
      {
         // Still moving, check again soon
         schedule(500, 0, "checkGolfStop", %client);
      }
   }
}

//-----------------------------------------------------------------------------
// Golf Hole Trigger
//-----------------------------------------------------------------------------

datablock TriggerData(SMBGolfHoleTrigger)
{
   tickPeriodMS = 100;
};

function SMBGolfHoleTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if ($Game::MonkeyGolfActive && %obj.getClassName() $= "Marble")
   {
      if (isObject(%obj.client))
      {
         %client = %obj.client;

         // Finished!
         messageClient(%client, 'MsgSystem', '\c0Hole completed in %1 strokes!', %client.score);
         PartyGame::endGameUI(%client, "<color:00ff00><font:Arial Bold:24>Hole in " @ %client.score @ "!");

         if (isObject(pickupSfx))
            serverPlay3D(pickupSfx, %trigger.getTransform());

         // Freeze the marble
         %obj.setMode(2);

         // Reset for next hole/round
         schedule(3000, 0, "serverCmdRestartLevel", %client);
      }
   }
}
