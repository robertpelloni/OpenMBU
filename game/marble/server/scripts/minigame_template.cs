//-----------------------------------------------------------------------------
// Super Monkey Ball: Generic Minigame Template
//-----------------------------------------------------------------------------

// This file provides generic wrapper functions for common minigame tasks
// such as updating UI, managing scores, and handling basic player states.

// Call this from a specific minigame's onStart()
function MinigameTemplate::init(%minigameName)
{
   $Game::ActiveMinigameName = %minigameName;
   echo("Minigame Template Initialized for: " @ %minigameName);
}

// Universal Score Updater
function MinigameTemplate::addScore(%client, %points)
{
   if (isObject(%client))
   {
      %client.minigameScore += %points;

      // Default to picking up sound if available
      if (isObject(pickupSfx))
      {
         serverPlay3D(pickupSfx, %client.player.getTransform());
      }

      return %client.minigameScore;
   }
   return 0;
}

// Universal UI Updater
function MinigameTemplate::updateUI(%client, %statusText, %durationSecs)
{
   if (isObject(%client))
   {
      %timeMS = %durationSecs * 1000;
      if (%timeMS <= 0) %timeMS = 0; // 0 means persistent until overridden

      %scoreText = "Score: " @ %client.minigameScore;
      bottomPrint(%client, $Game::ActiveMinigameName @ " - " @ %statusText @ "<br>" @ %scoreText, %timeMS, 2);
   }
}

// Example Generic Minigame
function ExampleMinigame::onStart()
{
   MinigameTemplate::init("Example Game");
}
