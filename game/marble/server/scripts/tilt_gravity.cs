//-----------------------------------------------------------------------------
// Super Monkey Ball Style Tilt Gravity Mechanics
//-----------------------------------------------------------------------------

// Toggle for gravity tilt mode
$Game::TiltGravityMode = true;

function serverCmdToggleTiltGravity(%client)
{
   $Game::TiltGravityMode = !$Game::TiltGravityMode;
   if ($Game::TiltGravityMode)
      messageClient(%client, 'MsgSystem', '\c0Tilt Gravity Mode: ENABLED');
   else
      messageClient(%client, 'MsgSystem', '\c0Tilt Gravity Mode: DISABLED');
}

// Hook into marble update or player update to tilt the gravity vector
// Since this is TorqueScript, we can alter the global gravity vector based on player input.
// Note: This changes global gravity, affecting all physics objects, which perfectly emulates SMB!

function updateTiltGravity(%client, %moveX, %moveY)
{
   if (!$Game::TiltGravityMode)
      return;

   // Base gravity
   %baseZ = -19.62;

   // Calculate tilt based on input (moveX/moveY are typically -1 to 1)
   // We want a max tilt of maybe 20 degrees
   %maxTilt = 0.3; // Multiplier for gravity vector

   %gravX = %moveX * %baseZ * %maxTilt;
   %gravY = %moveY * %baseZ * %maxTilt;
   %gravZ = %baseZ;

   // Set global gravity (Requires exposing setGravity to script if not already)
   if (isObject(%client.player))
   {
      %client.player.setGravityDir(%gravX @ " " @ %gravY @ " " @ %gravZ, false);
   }
}

// In a real implementation, we would hook this into the move map or marble update tick.
// We schedule it to run.
function tiltGravityLoop()
{
   cancel($TiltGravitySchedule);
   if ($Game::TiltGravityMode)
   {
      for (%i = 0; %i < ClientGroup.getCount(); %i++)
      {
         %client = ClientGroup.getObject(%i);
         if (isObject(%client.player))
         {
            // We'll read the move map input here... well, server side doesn't have the move input directly accessible
            // easily unless passed through commands. MBU uses move logic in C++.
            // So we can send a command from client.
         }
      }
   }
   $TiltGravitySchedule = schedule(32, 0, tiltGravityLoop);
}
//tiltGravityLoop(); // Temporarily disabled if we just hook it via client cmds

function serverCmdUpdateTiltGravity(%client, %moveX, %moveY)
{
   updateTiltGravity(%client, %moveX, %moveY);
}
