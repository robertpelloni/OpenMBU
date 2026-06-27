//-----------------------------------------------------------------------------
// Super Monkey Ball: Billiards Minigame
//-----------------------------------------------------------------------------

datablock RigidShapeData(SMBBilliardBall)
{
   category = "SMB Minigames";
   className = "BilliardBall";
   shapeFile = "~/data/shapes/balls/marble01.dts"; // Use a standard marble shape

   mass = 1.0;
   friction = 0.1;       // Very low friction for rolling on felt
   restitution = 0.95;   // Extremely bouncy against other balls and rails
};

function BilliardsMinigame::onStart()
{
   echo("Billiards Minigame initialized!");
   $Game::BilliardsState = "Aiming"; // States: Aiming, Rolling
}

function BilliardsMinigame::onEnd()
{
   echo("Billiards Minigame shutting down!");
}

function BilliardsMinigame::onPlayerJoin(%client)
{
   %client.score = 0;
   messageClient(%client, 'MsgSystem', '\c0Welcome to Billiards! Sink the balls into the pockets.');
}

function BilliardsMinigame::onPlayerSpawn(%player)
{
   // In aiming phase, restrict standard rolling movement
   %player.setMode(2);
   $Game::BilliardsState = "Aiming";

   // We would normally spawn the rack of balls here based on the level design.
}

// Custom billiards command triggered by a specific action key
function serverCmdBilliardsStrike(%client, %powerX, %powerY)
{
   if ($Game::BilliardsState $= "Aiming" && isObject(%client.player))
   {
      $Game::BilliardsState = "Rolling";

      // Apply impulse based on the 2D aim vector provided by the client's UI/Camera
      %client.player.setMode(1); // Normal movement
      %client.player.applyImpulse("0 0 0", %powerX SPC %powerY SPC "0");

      // We would schedule a check to see when all balls stop moving to return to "Aiming"
      schedule(5000, 0, "checkBilliardsTurnEnd", %client);
   }
}

function checkBilliardsTurnEnd(%client)
{
   // A robust implementation would iterate over all RigidShapes and verify their velocity < epsilon.
   // For the prototype, we just wait a fixed time and revert state.
   $Game::BilliardsState = "Aiming";
   if (isObject(%client.player))
   {
      %client.player.setMode(2); // Freeze again for next shot
      messageClient(%client, 'MsgSystem', '\c0Balls have settled. Take your next shot!');
   }
}

// Pocket trigger logic
datablock TriggerData(SMBPocketTrigger)
{
   tickPeriodMS = 100;
};

function SMBPocketTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "RigidShape" && %obj.getDataBlock().getName() $= "SMBBilliardBall")
   {
      // Ball sank!
      %obj.delete();
      // Add score to the active player (simplistic global track for prototype)
      for (%i = 0; %i < ClientGroup.getCount(); %i++)
      {
         %client = ClientGroup.getObject(%i);
         %client.score += 10;
         messageClient(%client, 'MsgSystem', '\c0Ball Sunk! +10 Points');
      }
   }
   else if (%obj.getClassName() $= "Marble")
   {
      // Scratch! Player sank the cue ball
      %client = %obj.client;
      messageClient(%client, 'MsgSystem', '\c0SCRATCH! You sank the cue ball!');
      // Typically reset cue ball position here
      schedule(1000, 0, "serverCmdRestartLevel", %client);
   }
}
