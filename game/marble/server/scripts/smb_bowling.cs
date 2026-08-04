//-----------------------------------------------------------------------------
// Super Monkey Ball: Bowling Minigame
//-----------------------------------------------------------------------------

datablock RigidShapeData(SMBBowlingPin)
{
   category = "SMB Minigames";
   className = "BowlingPin";
   shapeFile = "~/data/shapes/items/gem.dts"; // Placeholder shape

   mass = ($Game::MonkeyBowling::PinMass !$= "") ? $Game::MonkeyBowling::PinMass : 1.5;
   friction = ($Game::MonkeyBowling::PinFriction !$= "") ? $Game::MonkeyBowling::PinFriction : 0.2;
   restitution = ($Game::MonkeyBowling::PinRestitution !$= "") ? $Game::MonkeyBowling::PinRestitution : 0.5;

   // A bowling pin should have a higher center of mass to easily tip
   massCenter = "0 0 0.5";
};

function BowlingMinigame::onStart()
{
   echo("Bowling Minigame initialized!");
   $Game::BowlingState = "Aiming"; // States: Aiming, Rolling, Scoring
   $Game::MonkeyBowlingActive = true;
}

function BowlingMinigame::onEnd()
{
   echo("Bowling Minigame shutting down!");
   $Game::MonkeyBowlingActive = false;
}

function BowlingMinigame::onPlayerJoin(%client)
{
   PartyGame::initClientScore(%client, '\c0Welcome to Bowling! Strike it big!');
}

function BowlingMinigame::onPlayerSpawn(%player)
{
   // In aiming phase, we restrict movement to the X-axis
   %player.setMode(2); // Using existing restrictive modes or we can do it via tick
   $Game::BowlingState = "Aiming";
   %player.client.currentPins = 10;

   spawnPins();
}

// Custom bowling command triggered by a specific action key (e.g. Trigger 3)
function serverCmdBowlingThrow(%client, %power)
{
   if ($Game::BowlingState $= "Aiming" && isObject(%client.player))
   {
      $Game::BowlingState = "Rolling";
      // Apply massive forward impulse to emulate throw
      %client.player.setMode(1); // Normal movement
      %powerMult = ($Game::MonkeyBowling::StrikePowerMult !$= "") ? $Game::MonkeyBowling::StrikePowerMult : 50;

      %forwardVec = %client.player.getForwardVector();
      %impulseVec = VectorScale(%forwardVec, %power * %powerMult);
      %client.player.applyImpulse("0 0 0", %impulseVec);

      // Schedule score calculation
      %delay = ($Game::MonkeyBowling::ScoreDelayMS !$= "") ? $Game::MonkeyBowling::ScoreDelayMS : 5000;
      schedule(%delay, 0, "calculateBowlingScore", %client);
   }
}

function spawnPins()
{
   // Find the pin anchor (e.g. a Path or Marker) or hardcode relative to lane
   %anchorPos = "0 50 0"; // Example lane end

   // Simple triangle formation
   %pinId = 0;
   for (%row = 0; %row < 4; %row++)
   {
      for (%col = 0; %col <= %row; %col++)
      {
         %x = (%col * 1.5) - (%row * 0.75);
         %y = %row * 1.5;

         %pin = new RigidShape() {
            dataBlock = "SMBBowlingPin";
            position = VectorAdd(%anchorPos, %x SPC %y SPC "0.5");
            pinId = %pinId;
         };
         MissionCleanup.add(%pin);
         %pinId++;
      }
   }
}

function calculateBowlingScore(%client)
{
   %knockedOver = 0;

   // Iterate over all pins and check their Z up-vector
   for (%i = 0; %i < MissionCleanup.getCount(); %i++)
   {
      %obj = MissionCleanup.getObject(%i);
      if (%obj.getClassName() $= "RigidShape" && %obj.getDataBlock().getName() $= "SMBBowlingPin")
      {
         // A tipped pin's Z-axis will deviate from 1.0
         %transform = %obj.getTransform();
         %upVectorZ = getWord(%transform, 5); // Simplistic check depending on matrix layout

         // In Torque, transform is PosX PosY PosZ AxisX AxisY AxisZ Angle
         // To accurately check tipping, we really need the up vector from the matrix.
         // If a pin is knocked over, the angle of the Z axis will deviate significantly.

         %axisX = getWord(%transform, 3);
         %axisY = getWord(%transform, 4);
         %axisZ = getWord(%transform, 5);
         %angle = getWord(%transform, 6);

         %pos = %obj.getPosition();

         // Pin is knocked over if it has fallen off the lane (Z < -5)
         // OR if its rotation angle suggests it's tipped over (e.g. angle > 0.5 rad on XY axes)

         %isTipped = false;

         // If axis is largely X or Y, and angle is significant, it's tipped.
         if (mAbs(%axisZ) < 0.8 && %angle > 0.5)
            %isTipped = true;

         if (getWord(%pos, 2) < -5 || %isTipped)
         {
            %knockedOver++;
            // Optionally remove or disable the knocked pin
            // %obj.delete(); // Keep it for visual though
         }
      }
   }

   %client.score += %knockedOver;
   messageClient(%client, 'MsgSystem', '\c0You knocked over %1 pins! Total Score: %2', %knockedOver, %client.score);
   PartyGame::endGameUI(%client, "<color:00ff00><font:Arial Bold:24>Total Score: " @ %client.score);

   // Reset round
   schedule(2000, 0, "serverCmdRestartLevel", %client);
}
