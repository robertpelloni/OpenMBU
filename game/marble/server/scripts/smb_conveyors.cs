//-----------------------------------------------------------------------------
// Super Monkey Ball: Conveyor Belts
//-----------------------------------------------------------------------------

// A Conveyor uses a Trigger volume to push the marble continuously in a specific direction.

datablock TriggerData(SMBConveyorTrigger)
{
   tickPeriodMS = 32; // Fast tick for smooth pushing
};

function SMBConveyorTrigger::onTickTrigger(%this, %trigger)
{
   // Push all objects currently inside the trigger
   for (%i = 0; %i < %trigger.getNumObjects(); %i++)
   {
      %obj = %trigger.getObject(%i);
      if (%obj.getClassName() $= "Marble")
      {
         // Default direction is forward (+Y)
         %dir = (%trigger.pushDir !$= "") ? %trigger.pushDir : "0 1 0";

         // Default force from global config or fallback
         %baseForce = ($Game::Obstacles::ConveyorForce !$= "") ? $Game::Obstacles::ConveyorForce : 15;
         %force = (%trigger.pushForce !$= "") ? %trigger.pushForce : %baseForce;

         // Apply continuous impulse
         %impulse = VectorScale(%dir, %force * 0.032); // Scale by tick rate
         %obj.applyImpulse("0 0 0", %impulse);
      }
   }
}

// Dynamic spawning helper
function createConveyor(%position, %scale, %pushDir, %pushForce)
{
   if (%scale $= "") %scale = "1 1 1";
   if (%pushDir $= "") %pushDir = "0 1 0";
   if (%pushForce $= "") %pushForce = ($Game::Obstacles::ConveyorForce !$= "") ? $Game::Obstacles::ConveyorForce : 15;

   %conveyor = new Trigger()
   {
      dataBlock = "SMBConveyorTrigger";
      position = %position;
      scale = %scale;
      polyhedron = "0.0000000 0.0000000 0.0000000 1.0000000 0.0000000 0.0000000 0.0000000 -1.0000000 0.0000000 0.0000000 0.0000000 1.0000000";
      pushDir = %pushDir;
      pushForce = %pushForce;
   };

   MissionGroup.add(%conveyor);
   return %conveyor;
}
