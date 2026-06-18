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
   // core()->setGravity("0 0 -19.62");
   // MBU uses setGravity(%x, %y, %z);
   setGravity(%gravX, %gravY, %gravZ);
}

// In a real implementation, we would hook this into the move map or marble update tick.
