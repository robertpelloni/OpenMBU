//-----------------------------------------------------------------------------
// Super Monkey Ball: Monkey Target Prototype
//-----------------------------------------------------------------------------

$Game::MonkeyTargetMode = false;

// Glider datablock config
datablock MarbleData(GliderMarble : DefaultMarble)
{
   gravity = 5; // Reduced gravity for gliding
   airAcceleration = 25.0; // High air control
   maxRollVelocity = 25; // Faster airborne
};

function serverCmdToggleMonkeyTarget(%client)
{
   $Game::MonkeyTargetMode = !$Game::MonkeyTargetMode;
   if ($Game::MonkeyTargetMode)
   {
      messageClient(%client, 'MsgSystem', '\c0Monkey Target Mode: ENABLED');
      %client.player.isGliding = false;
   }
   else
   {
      messageClient(%client, 'MsgSystem', '\c0Monkey Target Mode: DISABLED');
      %client.player.isGliding = false;
      %client.player.setDataBlock(DefaultMarble);
   }
}

// Monkey Target uses TriggerNum 1 (altTrigger / blast usually) to toggle wings
// We hook into MarbleData::onTrigger which we already modified for jumping (TriggerNum 2).
// Since MarbleData::onTrigger in MBU is used for powerups (trigger 0 typically) and blast,
// we can intercept it if MonkeyTargetMode is active.

// We will overwrite the main onTrigger locally or add a hook.

// Target Zones for Monkey Target
datablock TriggerData(SMBTargetZoneTrigger)
{
   tickPeriodMS = 100;
};

// Each zone provides points and ends the flight attempt
function SMBTargetZoneTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble" && $Game::MonkeyTargetMode)
   {
      // Extract points from the trigger's dynamic field (e.g. %trigger.points)
      %points = (%trigger.points !$= "") ? %trigger.points : 100;

      // Add points
      %client = %obj.client;
      if (isObject(%client))
      {
         %client.score += %points;
         messageClient(%client, 'MsgSystem', '\c0Monkey Target Landed! Points: %1', %points);
      }

      // Revert glider and end round
      %obj.isGliding = false;
      %obj.setDataBlock(DefaultMarble);

      // Optionally reset the level or marble to the start for the next attempt
      // For now, we'll just freeze them briefly and log the score.
      %obj.setMode(0); // Freeze mode
      schedule(2000, 0, "serverCmdRestartLevel", %client);
   }
}
