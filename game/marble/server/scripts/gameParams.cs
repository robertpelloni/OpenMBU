//-----------------------------------------------------------------------------
// Super Monkey Ball: Centralized Config Variables
//-----------------------------------------------------------------------------

// General Game Mechanics
$Game::JumpForce = 7.5;

// Physics Overhaul: World-Tilt Gravity
$Game::UseWorldTilt = true;
$Game::TiltBlend = 1.0; // 1.0 = full world tilt, 0.0 = full direct torque input

// Party Minigame: Monkey Target
$Game::MonkeyTarget::DefaultPoints = 100;
$Game::MonkeyTarget::ResetDelayMS = 2000;
$Game::MonkeyTarget::GliderGravity = 5;
$Game::MonkeyTarget::GliderAirAccel = 25.0;
$Game::MonkeyTarget::GliderMaxRoll = 25;

// Party Minigame: Monkey Bowling
$Game::MonkeyBowling::ScoreDelayMS = 5000;
$Game::MonkeyBowling::StrikePowerMult = 50;
