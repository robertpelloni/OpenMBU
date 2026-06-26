//-----------------------------------------------------------------------------
// Super Monkey Ball Bumpers
//-----------------------------------------------------------------------------

datablock SFXProfile(SMBBumperHitSfx)
{
   filename    = "~/data/sound/bumperDing1.wav"; // Reusing default for now
   description = AudioDefault3d;
   preload = true;
};

// Create a new material mapping for Super Monkey Ball Bumper
// We can define it in script if we want, or just reuse BumperMaterial.
// Let's create a specific one if we want a stronger force
new MaterialProperty(SMBBumperMaterial) {
   friction = 0.5;
   restitution = 0;
   force = 25; // Stronger than standard bumper (15)
};
addMaterialMapping("smb_bumper_mat", SMBBumperMaterial);

datablock StaticShapeData(SMBRoundBumper)
{
   category = "SMB Obstacles";
   className = "Bumper";
   shapeFile = "~/data/shapes/bumpers/pball_round.dts"; // Reuse shape, map texture in modeling ideally, but we can override or just reuse.
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
   %obj.stopThread(0);
   %obj.playThread(0, "activate");
   %obj.playAudio(0, %this.sound);

   // Ensure a strong bounce even if material isn't mapped properly
   if (%col.getClassName() $= "Marble")
   {
      // Calculate outward vector from bumper center to marble
      %bumperPos = %obj.getPosition();
      %marblePos = %col.getPosition();

      %outward = VectorSub(%marblePos, %bumperPos);
      // Flatten the Z to keep bounce mostly horizontal
      %outward = setWord(%outward, 2, 0);
      %outward = VectorNormalize(%outward);

      // Apply impulse
      %force = 20; // Bumper strength
      %impulse = VectorScale(%outward, %force);
      %col.applyImpulse("0 0 0", %impulse);
   }
}
