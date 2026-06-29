//-----------------------------------------------------------------------------
// Super Monkey Ball: Giant Ape Boss Prototype
//-----------------------------------------------------------------------------

datablock StaticShapeData(GiantApeBossShape)
{
   category = "Bosses";
   shapeFile = "~/data/shapes/hazards/tornado/tornado.dts"; // Placeholder shape
};

function ApeBoss::onSpawn(%state, %obj)
{
   echo("Giant Ape has appeared!");
   %obj.setDataBlock(GiantApeBossShape);
   // Initial scaling or effects
   %obj.setScale("5 5 5");
}

function ApeBoss::onUpdate(%state)
{
   %obj = %state.physicalObject;

   // Extremely basic AI: Rotate towards player
   // In a real scenario, we'd grab the closest player.
   // For prototype, just spin.

   %rot = %obj.getRotation();
   // Simple rotation on Z axis
   %newRot = VectorAdd(%rot, "0 0 5");
   %obj.setRotation(%newRot);

   // Phase 2 logic
   if (%state.phase == 2)
   {
      // Move faster or spawn hazards
      %obj.setScale("7 7 7"); // Enrage mode
   }
}

function ApeBoss::onPhaseChange(%state, %newPhase)
{
   if (%newPhase == 2)
   {
      for (%i = 0; %i < ClientGroup.getCount(); %i++)
      {
         %client = ClientGroup.getObject(%i);
         messageClient(%client, 'MsgSystem', '\c0The Giant Ape is ENRAGED!');
      }

      // Flash red or play sound
      if (isObject(PowerUpSfx))
         serverPlay3D(PowerUpSfx, %state.physicalObject.getTransform());
   }
}

function ApeBoss::onDeath(%state)
{
   // Play death explosion, drop bananas, etc.
   if (isObject(ExplosionSfx))
      serverPlay3D(ExplosionSfx, %state.physicalObject.getTransform());

   // Spawn 10 bananas as a reward
   %pos = %state.physicalObject.getPosition();
   for (%i = 0; %i < 10; %i++)
   {
      %banana = new Item() {
         dataBlock = "BananaItem";
         position = VectorAdd(%pos, getRandom(-5, 5) SPC getRandom(-5, 5) SPC "5");
      };
      MissionCleanup.add(%banana);
   }
}

// Dev command to spawn and test the boss
function serverCmdSpawnApeBoss(%client)
{
   if (isObject(%client.player))
   {
      %pos = %client.player.getPosition();
      %spawnPos = VectorAdd(%pos, "10 0 5"); // Spawn in front/above player
      spawnBoss("Giant Ape", %spawnPos, "ApeBoss");
   }
}

// Dev command to damage the boss
function serverCmdDamageApeBoss(%client)
{
   // Find the active boss (simplistic, assumes 1 boss)
   for (%i = 0; %i < MissionCleanup.getCount(); %i++)
   {
      %obj = MissionCleanup.getObject(%i);
      if (%obj.class $= "BossState" && %obj.bossType $= "ApeBoss")
      {
         damageBoss(%obj, 25);
         break;
      }
   }
}
