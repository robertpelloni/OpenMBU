//-----------------------------------------------------------------------------
// Super Monkey Ball: Boss System Framework
//-----------------------------------------------------------------------------

// The Boss Framework manages boss lifecycle, phases, and health tracking.
// A "Boss" in this context is typically a script-driven RigidShape or AIPlayer
// depending on what exists in the engine. OpenMBU has Marble and RigidShape.
// We can use a generic script object to track state and manipulate an underlying ShapeBase.

function spawnBoss(%bossName, %position, %type)
{
   echo("Spawning Boss: " @ %bossName @ " of type " @ %type);

   // Create the state tracker
   %bossState = new ScriptObject()
   {
      class = "BossState";
      bossName = %bossName;
      bossType = %type;
      health = 100;
      maxHealth = 100;
      phase = 1;
      target = 0; // Usually the player
   };

   // Create the physical representation (using a StaticShape or RigidShape if available)
   // We'll assume a generic datablock exists or will be provided by the specific boss script.
   %bossObj = new StaticShape()
   {
      dataBlock = "DefaultBossShape"; // Needs to be defined by the specific boss
      position = %position;
      stateTracker = %bossState;
   };

   %bossState.physicalObject = %bossObj;

   MissionCleanup.add(%bossState);
   MissionCleanup.add(%bossObj);

   // Call the specific boss's onSpawn hook
   if (isFunction(%type, "onSpawn"))
   {
      eval(%type @ "::onSpawn(" @ %bossState @ ", " @ %bossObj @ ");");
   }

   // Start the boss loop
   %bossState.loopSchedule = schedule(100, 0, "bossLoop", %bossState);

   return %bossState;
}

function bossLoop(%bossState)
{
   if (!isObject(%bossState) || !isObject(%bossState.physicalObject))
      return;

   // Call specific boss AI/Update tick
   if (isFunction(%bossState.bossType, "onUpdate"))
   {
      eval(%bossState.bossType @ "::onUpdate(" @ %bossState @ ");");
   }

   %bossState.loopSchedule = schedule(100, 0, "bossLoop", %bossState);
}

function damageBoss(%bossState, %amount)
{
   if (!isObject(%bossState)) return;

   %bossState.health -= %amount;
   echo("Boss " @ %bossState.bossName @ " took damage! Health: " @ %bossState.health);

   // Broadcast health update
   // Use bottomPrint for a more prominent "Boss UI" feel
   for (%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %client = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgSystem', '\c0BOSS HEALTH: %1 / %2', %bossState.health, %bossState.maxHealth);
      bottomPrint(%client, "<color:ff0000><font:Arial Bold:24>BOSS HEALTH: " @ %bossState.health @ " / " @ %bossState.maxHealth, 3, 2);
   }

   // Check Phase Transition
   if (%bossState.health <= %bossState.maxHealth * 0.5 && %bossState.phase == 1)
   {
      %bossState.phase = 2;
      echo("Boss entering Phase 2!");
      if (isFunction(%bossState.bossType, "onPhaseChange"))
         eval(%bossState.bossType @ "::onPhaseChange(" @ %bossState @ ", 2);");
   }

   // Check Death
   if (%bossState.health <= 0)
   {
      killBoss(%bossState);
   }
}

function killBoss(%bossState)
{
   echo("Boss " @ %bossState.bossName @ " Defeated!");
   cancel(%bossState.loopSchedule);

   if (isFunction(%bossState.bossType, "onDeath"))
      eval(%bossState.bossType @ "::onDeath(" @ %bossState @ ");");

   // Cleanup
   if (isObject(%bossState.physicalObject))
      %bossState.physicalObject.delete();

   for (%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %client = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgSystem', '\c0BOSS DEFEATED! YOU WIN!');
   }

   %bossState.delete();
}
