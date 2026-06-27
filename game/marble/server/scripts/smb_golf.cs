//-----------------------------------------------------------------------------
// Super Monkey Ball: Golf Minigame
//-----------------------------------------------------------------------------

function GolfMinigame::onStart()
{
   echo("Golf Minigame initialized!");
   $Game::GolfState = "Aiming"; // States: Aiming, Putting, Moving
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
   %player.setMode(2);
   $Game::GolfState = "Aiming";
}

// Command called by the client UI after charging the putt meter
function serverCmdGolfPutt(%client, %powerX, %powerY)
{
   if ($Game::GolfState $= "Aiming" && isObject(%client.player))
   {
      $Game::GolfState = "Moving";
      %client.strokes++;

      messageClient(%client, 'MsgSystem', '\c0Stroke %1', %client.strokes);

      %client.player.setMode(1); // Normal movement
      %client.player.applyImpulse("0 0 0", %powerX SPC %powerY SPC "0");

      // Schedule check to halt marble when it stops
      schedule(3000, 0, "checkGolfBallSettled", %client);
   }
}

function checkGolfBallSettled(%client)
{
   if (!isObject(%client.player)) return;

   // In Torque, we can check velocity. For prototype, just freeze it after a timer.
   // An accurate system would poll %client.player.getVelocity() until it's near zero.
   $Game::GolfState = "Aiming";
   %client.player.setMode(2);
   messageClient(%client, 'MsgSystem', '\c0Ball settled. Line up your next shot.');
}

// Golf Hole logic
datablock TriggerData(SMBHoleTrigger)
{
   tickPeriodMS = 100;
};

function SMBHoleTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble")
   {
      %client = %obj.client;
      if (isObject(%client))
      {
         messageClient(%client, 'MsgSystem', '\c0HOLE IN %1!', %client.strokes);
         // End the hole, tally score, move to next hole
         schedule(3000, 0, "serverCmdRestartLevel", %client); // Reset for prototype loop
      }
   }
}
