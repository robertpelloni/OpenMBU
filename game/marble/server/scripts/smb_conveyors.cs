//-----------------------------------------------------------------------------
// Super Monkey Ball: Conveyor Belts
//-----------------------------------------------------------------------------

// A Conveyor uses a Trigger volume to push the marble continuously in a specific direction.
// The direction and force are defined on the trigger instance in the mission editor
// using dynamic fields: %trigger.pushDir and %trigger.pushForce.

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

         // Default force
         %force = (%trigger.pushForce !$= "") ? %trigger.pushForce : 15;

         // Apply continuous impulse
         %impulse = VectorScale(%dir, %force * 0.032); // Scale by tick rate
         %obj.applyImpulse("0 0 0", %impulse);
      }
   }
}
