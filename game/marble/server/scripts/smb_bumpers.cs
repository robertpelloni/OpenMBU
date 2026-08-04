//-----------------------------------------------------------------------------
// Super Monkey Ball Bumpers
//-----------------------------------------------------------------------------

datablock SFXProfile(SMBBumperHitSfx)
{
   filename    = "~/data/sound/bumperDing1.wav";
   description = AudioDefault3d;
   preload = true;
};

// Material mapping for Super Monkey Ball Bumper
new MaterialProperty(SMBBumperMaterial) {
   friction = 0.5;
   restitution = 0;
   // We will apply custom impulse via script instead of relying purely on material force
   force = 0;
};
addMaterialMapping("smb_bumper_mat", SMBBumperMaterial);

datablock StaticShapeData(SMBRoundBumper)
{
   category = "SMB Obstacles";
   className = "Bumper";
   shapeFile = "~/data/shapes/bumpers/pball_round.dts";
   scopeAlways = true;
   sound = SMBBumperHitSfx;
};

function SMBRoundBumper::onAdd(%this, %obj)
{
   %obj.playThread(0, "idle");
}

function SMBRoundBumper::onEndSequence(%this, %obj, %slot)
{
   %obj.stopThread(0);
   %obj.playThread(0, "idle");
}

function SMBRoundBumper::onCollision(%this, %obj, %col, %vec, %vecLen, %material)
{
   if (%col.getClassName() $= "Marble")
   {
      %obj.stopThread(0);
      %obj.playThread(0, "activate");
      if (isObject(%this.sound))
         %obj.playAudio(0, %this.sound);

      // Apply explicit impulse as per Memory.md to overcome .dts material limits
      %force = ($Game::Obstacles::BumperForce !$= "") ? $Game::Obstacles::BumperForce : 25;

      // Calculate repulsion vector (away from bumper center)
      %bumpPos = %obj.getPosition();
      %colPos = %col.getPosition();
      %dir = VectorNormalize(VectorSub(%colPos, %bumpPos));
      %impulse = VectorScale(%dir, %force);

      %col.applyImpulse("0 0 0", %impulse);
   }
}

// Dynamic spawning helper
function createBumper(%position, %scale)
{
   if (%scale $= "") %scale = "1 1 1";

   %bumper = new StaticShape()
   {
      dataBlock = "SMBRoundBumper";
      position = %position;
      scale = %scale;
   };

   MissionGroup.add(%bumper);
   return %bumper;
}
