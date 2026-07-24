//-----------------------------------------------------------------------------
// Super Monkey Ball: General Obstacle Helpers
//-----------------------------------------------------------------------------

// This file serves as a consolidated entry point for dynamic obstacle spawning
// that might require composite structures or cross-module references.

// Create a wind tunnel (often used in Monkey Target or main campaign)
function createWindTunnel(%position, %scale, %windForce)
{
   if (%scale $= "") %scale = "1 1 1";
   if (%windForce $= "") %windForce = "0 0 15";

   %trigger = new Trigger()
   {
      dataBlock = "SMBWindTunnelTrigger";
      position = %position;
      scale = %scale;
      polyhedron = "0.0000000 0.0000000 0.0000000 1.0000000 0.0000000 0.0000000 0.0000000 -1.0000000 0.0000000 0.0000000 0.0000000 1.0000000";
      windForce = %windForce;
   };

   MissionGroup.add(%trigger);
   return %trigger;
}
