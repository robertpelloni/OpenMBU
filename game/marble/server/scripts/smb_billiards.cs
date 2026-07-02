//-----------------------------------------------------------------------------
// Super Monkey Ball: Billiards Minigame Stub
//-----------------------------------------------------------------------------

function BilliardsMinigame::onStart()
{
   MinigameTemplate::init("Monkey Billiards");
   echo("Billiards Minigame initialized!");
   // Here we would setup the table bounds, physics damping, cue ball state, etc.
}

function BilliardsMinigame::onEnd()
{
   echo("Billiards Minigame shutting down!");
}

function BilliardsMinigame::onPlayerJoin(%client)
{
   %client.minigameScore = 0;
   messageClient(%client, 'MsgSystem', '\c0Welcome to Billiards! Sink your opponent\'s balls.');
}

function BilliardsMinigame::onPlayerSpawn(%player)
{
   // Snap camera to top-down orthographic if possible, or lock to cue ball
   // Set physics to a custom flat-plane high-restitution style if needed

   // Enter aiming phase for cue ball
   %player.setMode(2); // Restrict movement
   %player.client.billiardsState = "Aiming";
   MinigameTemplate::updateUI(%player.client, "Aiming Cue Ball", 0);
}

// Custom billiard command to strike the cue ball
function serverCmdBilliardsStrike(%client, %power)
{
   if (%client.billiardsState $= "Aiming" && isObject(%client.player))
   {
      %client.billiardsState = "Rolling";

      %powerMult = ($Game::MonkeyBilliards::CuePowerMult !$= "") ? $Game::MonkeyBilliards::CuePowerMult : 40;
      %actualPower = %power * %powerMult;

      %client.player.setMode(1); // Normal movement
      %client.player.applyImpulse("0 0 0", "0" SPC %actualPower SPC "0"); // Forward vector

      MinigameTemplate::updateUI(%client, "Rolling...", 0);

      // Schedule check to see when it stops
      schedule(2000, 0, "checkBilliardsStop", %client);
   }
}

function checkBilliardsStop(%client)
{
   if (isObject(%client.player))
   {
      %vel = %client.player.getVelocity();
      %speed = VectorLen(%vel);

      if (%speed < 0.1)
      {
         // Ball stopped
         %client.billiardsState = "Aiming";
         %client.player.setMode(2); // Freeze again
         MinigameTemplate::updateUI(%client, "Aiming Cue Ball", 0);
      }
      else
      {
         schedule(500, 0, "checkBilliardsStop", %client);
      }
   }
}

// Trigger logic for Pockets
datablock TriggerData(SMBBilliardsPocketTrigger)
{
   tickPeriodMS = 100;
};

function SMBBilliardsPocketTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble")
   {
      %client = %obj.client;
      if (isObject(%client))
      {
         // If it's the cue ball, it's a scratch!
         messageClient(%client, 'MsgSystem', '\c0Scratch! Cue ball pocketed.');
         %obj.setMode(0); // Freeze mode
         schedule(1000, 0, "serverCmdRestartLevel", %client);
      }
      else
      {
         // It's a target ball, score it
         // Find the client who hit it last, or just award point to current turn if multiplayer
         // For now, if we have a simple single-player prototype:
         %playerClient = ClientGroup.getObject(0); // Simple hack for prototype
         if (isObject(%playerClient))
         {
            MinigameTemplate::addScore(%playerClient, 1);
            MinigameTemplate::updateUI(%playerClient, "Target Pocketed!", 3);
         }

         // Remove pocketed ball
         %obj.delete();
      }
   }
}
