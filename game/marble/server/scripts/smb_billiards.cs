//-----------------------------------------------------------------------------
// Super Monkey Ball: Billiards Minigame Stub
//-----------------------------------------------------------------------------

function BilliardsMinigame::onStart()
{
   echo("Billiards Minigame initialized!");
   $Game::BilliardsActive = true;
}

function BilliardsMinigame::onEnd()
{
   echo("Billiards Minigame shutting down!");
   $Game::BilliardsActive = false;
}

function BilliardsMinigame::onPlayerJoin(%client)
{
   %client.billiardsScore = 0;
   messageClient(%client, 'MsgSystem', '\c0Welcome to Monkey Billiards! Sink the balls to score points.');
}

function BilliardsMinigame::onPlayerSpawn(%player)
{
   %player.setMode(2); // Freeze mode for aiming
}

function serverCmdBilliardsShot(%client, %power)
{
   if ($Game::BilliardsActive && isObject(%client.player))
   {
      %powerMult = ($Game::Billiards::ShotPowerMult !$= "") ? $Game::Billiards::ShotPowerMult : 20;
      %actualPower = %power * %powerMult;

      // Calculate forward trajectory based on the player's camera/marble transform
      %transform = %client.player.getTransform();
      %forwardVec = %client.player.getForwardVector();
      %impulseVec = VectorScale(%forwardVec, %actualPower);

      %client.player.setMode(1); // Normal movement
      %client.player.applyImpulse("0 0 0", %impulseVec);

      messageClient(%client, 'MsgSystem', '\c0Shot fired! Power: %1', %power);
   }
}

datablock TriggerData(BilliardsPocketTrigger)
{
   tickPeriodMS = 100;
};

function BilliardsPocketTrigger::onEnterTrigger(%this, %trigger, %obj)
{
   if ($Game::BilliardsActive)
   {
      %scoreVal = ($Game::Billiards::PocketScore !$= "") ? $Game::Billiards::PocketScore : 10;

      if (%obj.getClassName() $= "RigidShape")
      {
         // Target ball sunk
         %obj.delete();

         // Give points to the last player who struck the cue ball (assuming single player for simplicity)
         // In multiplayer, you'd track this via collision, but we will award the local client or the host.
         if (ClientGroup.getCount() > 0)
         {
            %client = ClientGroup.getObject(0); // Primary client
            %client.billiardsScore += %scoreVal;
            messageClient(%client, 'MsgSystem', '\c0Ball Sunk! Score: %1', %client.billiardsScore);
            bottomPrint(%client, "<color:00ff00><font:Arial Bold:24>Score: " @ %client.billiardsScore, 3, 2);
         }

         if (isObject(pickupSfx))
            serverPlay3D(pickupSfx, %trigger.getTransform());
      }
      else if (%obj.getClassName() $= "Marble")
      {
         // Cue ball (player) sunk - scratch
         %client = %obj.client;
         if (isObject(%client))
         {
            %client.billiardsScore -= %scoreVal;
            messageClient(%client, 'MsgSystem', '\c0Scratch! Cue ball sunk. Score: %1', %client.billiardsScore);
            bottomPrint(%client, "<color:ff0000><font:Arial Bold:24>Scratch! Score: " @ %client.billiardsScore, 3, 2);
         }

         // Reset cue ball safely back to their last checkpoint/spawn rather than a hardcoded coordinate
         if (isObject(%client))
         {
            // The client object usually has a drop point or respawn logic in MBU
            %client.respawnPlayer();
         }
         else
         {
            %obj.setVelocity("0 0 0");
            %obj.setTransform("0 0 5"); // Fallback
         }

         %obj.setMode(2); // Freeze again for aiming

         if (isObject(DestroyedVoiceSfx))
            serverPlay3D(DestroyedVoiceSfx, %trigger.getTransform());
      }
   }
}
