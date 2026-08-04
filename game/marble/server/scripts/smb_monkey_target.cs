//-----------------------------------------------------------------------------
// Super Monkey Ball: Monkey Target Prototype
//-----------------------------------------------------------------------------

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
   echo("Monkey Target Minigame initialized!");
   $Game::MonkeyTargetActive = true;
}

function MonkeyTargetMinigame::onEnd()
{
   echo("Monkey Target Minigame shutting down!");
   $Game::MonkeyTargetActive = false;
}

function MonkeyTargetMinigame::onPlayerJoin(%client)
{
   PartyGame::initClientScore(%client, '\c0Monkey Target Mode Active! Fly to the targets.');
}

function MonkeyTargetMinigame::onPlayerSpawn(%player)
{
   %player.isGliding = false;
   %player.setDataBlock(DefaultMarble);
}

function serverCmdToggleMonkeyTarget(%client)
{
   $Game::MonkeyTargetActive = !$Game::MonkeyTargetActive;
   if ($Game::MonkeyTargetActive)
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

// Target Zones for Monkey Target
datablock TriggerData(SMBTargetZoneTrigger)
{
   tickPeriodMS = 100;
};

// Each zone provides points and ends the flight attempt
function SMBTargetZoneTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble" && $Game::MonkeyTargetActive)
   {
      // Extract points from the trigger's dynamic field (e.g. %trigger.points)
      %points = (%trigger.points !$= "") ? %trigger.points : $Game::MonkeyTarget::DefaultPoints;

      // Add points
      %client = %obj.client;
      if (isObject(%client))
      {
         %client.score += %points;
         PartyGame::endGameUI(%client, "<color:00ff00><font:Arial Bold:24>Landed! Points: " @ %points @ "<br>Total Score: " @ %client.score);
      }

      // Revert glider and end round
      %obj.isGliding = false;
      %obj.setDataBlock(DefaultMarble);

      // Play a sound for scoring
      if (isObject(pickupSfx))
      {
         serverPlay3D(pickupSfx, %obj.getTransform());
      }

      // Freeze mode
      %obj.setMode(0);

      // Delay restart
      %delay = ($Game::MonkeyTarget::ResetDelayMS !$= "") ? $Game::MonkeyTarget::ResetDelayMS : 2000;
      schedule(%delay, 0, "serverCmdRestartLevel", %client);
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
   if (%obj.getClassName() $= "Marble" && %obj.isGliding)
   {
      // Optional enter sound
   }
}

function SMBWindTunnelTrigger::onTickTrigger(%this, %trigger)
{
   for (%i = 0; %i < %trigger.getNumObjects(); %i++)
   {
      %obj = %trigger.getObject(%i);
      if (%obj.getClassName() $= "Marble" && %obj.isGliding)
      {
         // Get the force vector from the trigger's dynamic field, default to strong updraft
         %force = (%trigger.windForce !$= "") ? %trigger.windForce : "0 0 15";
         %obj.applyImpulse("0 0 0", %force);
      }
   }
}
