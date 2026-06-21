//-----------------------------------------------------------------------------
// Super Monkey Ball Switches & Gates
//-----------------------------------------------------------------------------

datablock SFXProfile(SMBSwitchSfx)
{
   filename    = "~/data/sound/custom/switch_press.wav";
   description = AudioDefault3d;
   preload = true;
};

// Push button that toggles a specific PathedInterior group or triggers an event
datablock StaticShapeData(SMBSwitch)
{
   category = "SMB Obstacles";
   className = "SMBSwitchClass";
   shapeFile = "~/data/shapes/buttons/pushbutton.dts"; // Reuse button shape
   scopeAlways = true;
   sound = SMBSwitchSfx;
};

function SMBSwitchClass::onAdd(%this, %obj)
{
   %obj.playThread(0, "up");
   %obj.isPressed = false;
}

function SMBSwitchClass::onCollision(%this, %obj, %col, %vec, %vecLen, %material)
{
   if (%col.getClassName() $= "Marble" && !%obj.isPressed)
   {
      %obj.isPressed = true;
      %obj.stopThread(0);
      %obj.playThread(0, "down");
      if(isObject(%this.sound))
         %obj.playAudio(0, %this.sound);

      // Trigger the gate or elevator mapped to this switch
      if (%obj.targetGroup !$= "")
      {
         %group = %obj.targetGroup;
         if (isObject(%group))
         {
            for (%i = 0; (%plat = %group.getObject(%i)) != -1; %i++)
            {
               if (%plat.getClassName() $= "PathedInterior")
               {
                  // Assuming -2 plays the path forward, or targetTime specifies position
                  %targetPos = (%obj.targetPos !$= "") ? %obj.targetPos : -2;
                  %plat.setTargetPosition(%targetPos);
               }
            }
         }
      }
   }
}

// Reset switch when marble leaves or mission resets
function SMBSwitchClass::reset(%this, %obj)
{
   %obj.isPressed = false;
   %obj.stopThread(0);
   %obj.playThread(0, "up");
}
