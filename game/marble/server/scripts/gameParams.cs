//-----------------------------------------------------------------------------
// Super Monkey Ball: Centralized Config Variables
//-----------------------------------------------------------------------------

// General Game Mechanics
$Game::JumpForce = 7.5;

// Collectables
$Game::Collectables::BananaLifeThreshold = 100;
$Game::Collectables::StartingLives = 3;
$Game::Collectables::BananaScoreValue = 10;

// Physics Overhaul: World-Tilt Gravity
$Game::UseWorldTilt = true;
$Game::TiltBlend = 1.0; // 1.0 = full world tilt, 0.0 = full direct torque input

// Party Minigame: Monkey Target
$Game::MonkeyTarget::DefaultPoints = 100;

// Party Minigame: Monkey Billiards
$Game::Billiards::ShotPowerMult = 20;
$Game::Billiards::PocketScore = 10;
$Game::MonkeyTarget::ResetDelayMS = 2000;
$Game::MonkeyTarget::GliderGravity = 5;
$Game::MonkeyTarget::GliderAirAccel = 25.0;
$Game::MonkeyTarget::GliderMaxRoll = 25;

// Party Minigame: Monkey Bowling
$Game::MonkeyBowling::ScoreDelayMS = 5000;
$Game::MonkeyBowling::StrikePowerMult = 50;
$Game::MonkeyBowling::PinMass = 1.5;
$Game::MonkeyBowling::PinFriction = 0.2;
$Game::MonkeyBowling::PinRestitution = 0.5;

// Party Minigame: Monkey Golf
$Game::MonkeyGolf::MaxPower = 100;
$Game::MonkeyGolf::PowerMult = 25;
$Game::MonkeyGolf::Par = 3;

// Party Minigame: Monkey Fight
$Game::MonkeyFight::PunchForce = 50;
$Game::MonkeyFight::PunchRadius = 10;

// Party Minigame: Monkey Race
$Game::MonkeyRace::LapsToWin = 3;

// SMB Obstacles
$Game::Obstacles::BumperForce = 25;
$Game::Obstacles::ConveyorForce = 15;
$Game::Obstacles::SeesawMass = 100.0;
