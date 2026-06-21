//-----------------------------------------------------------------------------
// Super Monkey Ball Warp Gates
//-----------------------------------------------------------------------------

datablock SFXProfile(SMBWarpSfx)
{
   filename    = "~/data/sound/custom/warp_gate.wav";
   description = AudioDefault3d;
   preload = true;
};

// Use a Trigger to act as the area of the Warp Gate
datablock TriggerData(SMBWarpGateTrigger)
{
   tickPeriodMS = 100;
};

function SMBWarpGateTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if (%obj.getClassName() $= "Marble")
   {
      // A warp gate should have a destination object (e.g. another trigger or a SpawnSphere)
      if (%trigger.destination !$= "")
      {
         %dest = %trigger.destination;
         if (isObject(%dest))
         {
            %destPos = %dest.getPosition();
            // Optional: Offset the position slightly so it doesn't instantly re-trigger
            // if the destination is also a warp gate
            %destPos = VectorAdd(%destPos, "0 0 1");

            // Warp the marble
            %obj.setPosition(%destPos, true); // true = reset velocities/snap, false = keep momentum?

            // In SMB, momentum is often kept, but sometimes not.
            // In Torque, setPosition with true warps physics.

            ServerPlay3D(SMBWarpSfx, %destPos);
         }
      }
   }
}
