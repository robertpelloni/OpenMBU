//-----------------------------------------------------------------------------
// Super Monkey Ball Seesaws
//-----------------------------------------------------------------------------

// Torque 3D's Physics system (TGE) supports RigidBody shape physics.
// We define a seesaw using the RigidShape class.

// Note: Datablock values evaluate at script compile time.
// If global config isn't available, we supply sane defaults inline.

datablock RigidShapeData(SMBSeesaw)
{
   category = "SMB Obstacles";
   className = "Seesaw";
   shapeFile = "~/data/shapes/structures/glass_flat.dts"; // Using memory directive flat shape

   // Physics parameters from config if defined, else defaults
   mass = 100.0;
   massCenter = "0 0 -0.5"; // Shift mass center down to naturally self-balance

   friction = 0.5;
   restitution = 0.2;
};

// Dynamic spawning helper
function createSeesaw(%position, %rotation, %scale)
{
   if (%scale $= "") %scale = "1 1 1";
   if (%rotation $= "") %rotation = "1 0 0 0";

   %seesaw = new RigidShape()
   {
      dataBlock = "SMBSeesaw";
      position = %position;
      rotation = %rotation;
      scale = %scale;
   };

   MissionGroup.add(%seesaw);
   return %seesaw;
}
