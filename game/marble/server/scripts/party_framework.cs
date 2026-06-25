//-----------------------------------------------------------------------------
// Super Monkey Ball Party Game Framework
//-----------------------------------------------------------------------------

// This framework routes core game events to specific minigames based on the
// MissionInfo.minigameType property defined in the .mis file.

$Game::MinigameActive = false;
$Game::CurrentMinigame = "";

function PartyFramework::init(%this)
{
   echo("--- Initializing Party Game Framework ---");
}

function PartyFramework::onMissionLoaded(%this)
{
   if (MissionInfo.minigameType !$= "")
   {
      $Game::MinigameActive = true;
      $Game::CurrentMinigame = MissionInfo.minigameType;

      echo("Starting Party Minigame: " @ $Game::CurrentMinigame);

      // Dispatch to specific minigame namespace
      %minigameClass = $Game::CurrentMinigame @ "Minigame";
      if (isFunction(%minigameClass, "onStart"))
         eval(%minigameClass @ "::onStart();");
   }
   else
   {
      $Game::MinigameActive = false;
      $Game::CurrentMinigame = "";
   }
}

function PartyFramework::onMissionEnded(%this)
{
   if ($Game::MinigameActive)
   {
      %minigameClass = $Game::CurrentMinigame @ "Minigame";
      if (isFunction(%minigameClass, "onEnd"))
         eval(%minigameClass @ "::onEnd();");
   }
}

function PartyFramework::onPlayerJoin(%this, %client)
{
   if ($Game::MinigameActive)
   {
      // Reset minigame score for client
      %client.minigameScore = 0;

      %minigameClass = $Game::CurrentMinigame @ "Minigame";
      if (isFunction(%minigameClass, "onPlayerJoin"))
         eval(%minigameClass @ "::onPlayerJoin(" @ %client @ ");");
   }
}

function PartyFramework::onPlayerSpawn(%this, %player)
{
   if ($Game::MinigameActive)
   {
      %minigameClass = $Game::CurrentMinigame @ "Minigame";
      if (isFunction(%minigameClass, "onPlayerSpawn"))
         eval(%minigameClass @ "::onPlayerSpawn(" @ %player @ ");");
   }
}

// Call init on load
PartyFramework::init();
