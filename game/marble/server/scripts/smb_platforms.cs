//-----------------------------------------------------------------------------
// Super Monkey Ball Elevators and Moving Platforms
//-----------------------------------------------------------------------------

// We reuse PathedInterior for moving platforms, but we can set up custom datablocks
// for different sound profiles and behaviors (e.g., continuous looping vs switch-activated)

datablock SFXProfile(SMBPlatformLoopSfx)
{
   filename = "~/data/sound/custom/platform_loop.wav";
   description = AudioClosestLooping3d;
   preload = true;
};

datablock PathedInteriorData(SMBMovingPlatform)
{
   sustainSound = SMBPlatformLoopSfx;
};

datablock PathedInteriorData(SMBElevator)
{
   sustainSound = SMBPlatformLoopSfx;
};

// Platforms in SMB often move constantly, which is handled natively by placing a
// PathedInterior in the mission editor and setting its target to -2.
// No extra code is explicitly needed here, but the datablocks organize them.
