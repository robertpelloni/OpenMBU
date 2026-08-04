//-----------------------------------------------------------------------------
// Super Monkey Ball Elevators and Moving Platforms
//-----------------------------------------------------------------------------

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
// We can wrap this in a dynamic helper for programmatic generation.

function createPlatform(%position, %pathIndex, %isElevator)
{
   %block = %isElevator ? "SMBElevator" : "SMBMovingPlatform";

   %plat = new PathedInterior()
   {
      dataBlock = %block;
      position = %position;
      interiorResource = "placeholder.dif"; // Must map to an actual .dif or .dts internally in editor
      pathIndex = %pathIndex;
   };

   MissionGroup.add(%plat);
   return %plat;
}
