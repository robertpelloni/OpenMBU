//-----------------------------------------------------------------------------
// Super Monkey Ball: Monkey Target Prototype
//-----------------------------------------------------------------------------

// Monkey Target Configuration Variables
// Values are now defined in gameParams.cs

// Glider datablock config
datablock MarbleData(GliderMarble : DefaultMarble)
{
   gravity = $Game::MonkeyTarget::GliderGravity; // Reduced gravity for gliding
   airAcceleration = $Game::MonkeyTarget::GliderAirAccel; // High air control
   maxRollVelocity = $Game::MonkeyTarget::GliderMaxRoll; // Faster airborne
};

// Framework Hooks
function MonkeyTargetMinigame::onStart()
{
   MinigameTemplate::init("Monkey Target");
   echo("Monkey Target Minigame initialized!");
   $Game::MonkeyTargetMode = true;
}

function MonkeyTargetMinigame::onEnd()
{
   echo("Monkey Target Minigame shutting down!");
   $Game::MonkeyTargetMode = false;
}

function MonkeyTargetMinigame::onPlayerJoin(%client)
{
   %client.minigameScore = 0;
   messageClient(%client, 'MsgSystem', '\c0Monkey Target Mode Active! Fly to the targets.');
   MinigameTemplate::updateUI(%client, "Fly to the targets!", 5);
}

function MonkeyTargetMinigame::onPlayerSpawn(%player)
{
   %player.isGliding = false;
   %player.setDataBlock(DefaultMarble);
}

function serverCmdToggleMonkeyTarget(%client)
{
   $Game::MonkeyTargetMode = !$Game::MonkeyTargetMode;
   if ($Game::MonkeyTargetMode)
   {
      messageClient(%client, 'MsgSystem', '\c0Monkey Target Mode: ENABLED');
      if (isObject(%client.player))
         %client.player.isGliding = false;
   }
   else
   {
      messageClient(%client, 'MsgSystem', '\c0Monkey Target Mode: DISABLED');
      if (isObject(%client.player))
      {
         %client.player.isGliding = false;
         %client.player.setDataBlock(DefaultMarble);
      }
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
      %points = (%trigger.points !$= "") ? %trigger.points : $Game::MonkeyTarget::DefaultPoints;

      // Add points via Template
      %client = %obj.client;
      if (isObject(%client))
      {
         MinigameTemplate::addScore(%client, %points);
         messageClient(%client, 'MsgSystem', '\c0Monkey Target Landed! Points: %1', %points);
         MinigameTemplate::updateUI(%client, "Landed! (+" @ %points @ ")", 3);
      }

      // Revert glider and end round
      %obj.isGliding = false;
      %obj.setDataBlock(DefaultMarble);

      // Optionally reset the level or marble to the start for the next attempt
      // For now, we'll just freeze them briefly and log the score.
      %obj.setMode(0); // Freeze mode
      schedule($Game::MonkeyTarget::ResetDelayMS, 0, "serverCmdRestartLevel", %client);
   }
}

//-----------------------------------------------------------------------------
// Wind Tunnel Obstacle for Monkey Target
//-----------------------------------------------------------------------------

datablock TriggerData(SMBWindTunnelTrigger)
{
   tickPeriodMS = 50; // Fast ticks for smooth physics force
};

function SMBWindTunnelTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble")
   {
      // We no longer strictly limit this to isGliding so it can be used
      // globally across other minigames (e.g. Golf, Billiards).
      // However, we can make the force much stronger for gliders.
   }
}

function SMBWindTunnelTrigger::onTickTrigger(%this, %trigger)
{
   for (%i = 0; %i < %trigger.getNumObjects(); %i++)
   {
      %obj = %trigger.getObject(%i);
      if (%obj.getClassName() $= "Marble")
      {
         // Get the force vector from the trigger's dynamic field, default to strong updraft
         %force = (%trigger.windForce !$= "") ? %trigger.windForce : "0 0 15";

         // If it's not gliding, dampen the effect slightly so standard marbles don't fly to space
         if (!%obj.isGliding)
         {
            %forceX = getWord(%force, 0) * 0.25;
            %forceY = getWord(%force, 1) * 0.25;
            %forceZ = getWord(%force, 2) * 0.25;
            %force = %forceX SPC %forceY SPC %forceZ;
         }

         %obj.applyImpulse("0 0 0", %force);
      }
   }
}
