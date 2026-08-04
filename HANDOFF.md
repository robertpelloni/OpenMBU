# Session Handoff Document

## Current Status
- Finished adding dynamic conveyor belts via `smb_conveyors.cs`. These rely on rapid triggering (`onTickTrigger`) to continuously push the marble.
- Jump mechanics within `marble.cs` have been refactored for tunable impact via `$Game::JumpForce`.
- Expanded `PartyFramework` (`party_framework.cs`) with new lifecycle hooks (`onClientLeaveGame`, `onPlayerDeath`, `onPlayerFinish`). Fixed a TorqueScript `isFunction` syntax issue in dynamic dispatch.
- Created `party_game_base.cs` providing a foundational namespace `PartyGame` with shared helper methods (`initClientScore`, `endGameUI`).
- Implemented `Monkey Fight` minigame (`smb_fight.cs`) featuring lives tracking, elimination handling, and a custom radius-based punch mechanic (`InitContainerRadiusSearch`).
- Implemented `Monkey Race` minigame (`smb_race.cs`) incorporating a lap counting system with checkpoint triggers.
- Implemented `Monkey Billiards` minigame (`smb_billiards.cs`), adding pocket trigger logic, cue ball scratch penalties, and physics-driven impulse shooting using the camera's forward vector.
- Updated `Monkey Bowling` minigame (`smb_bowling.cs`) to hook into the `PartyFramework` base helpers and state tracking. Improved its throw mechanic to utilize the player's camera forward vector rather than a hardcoded directional vector. Maintained existing pin-tipping logic (`mAbs(%axisZ) < 0.8 && %angle > 0.5`) to prevent false positives from vertical rotation. Sourced pin datablock physics parameters from `gameParams.cs`.
- Updated `Monkey Golf` minigame (`smb_golf.cs`) to hook into framework state and properly utilize camera forward vectors for swing mechanics instead of fixed-axis pushes. Hooked into `PartyGame` scoring logic and implemented `SMBGolfHoleTrigger` to handle sinking the ball.
- Integrated Collectables (Bananas). Refactored `banana.cs` to tie into global thresholds (`BananaLifeThreshold`, `BananaScoreValue`) defined in `gameParams.cs`. Initiated global player starting lives on client connection in `game.cs`.
- Finalized Hybrid World-Tilt Gravity. Implemented inverse delta matrix calculations inside the C++ `ConsoleMethod(Marble, setGravityDir)` to perfectly rotate and preserve the marble's relative linear and angular momentum (`mVelocity` and `mOmega`) when the physics frame shifts. This ensures seamless movement tracking regardless of the camera or gravity tilt angle.
- Completed the Obstacle Integration milestone by refactoring Bumpers, Switches, Warp Gates, Elevators, Conveyors, and Seesaws to be completely modular and dynamically spawnable (e.g. `createWarpGate`, `createBumper`). These hooks are configured to grab physics and force values directly from `gameParams.cs` and are centralized in `smb_obstacles.cs`.
- Submodules, logs, and Git hygiene have been strictly maintained.

## Next Steps for Successor Model
1. Now that the physics layer, minigames, and obstacles are complete, focus on the Boss System Framework or establishing robust `.mis` file generation logic to automatically compile these obstacles and minigames into a cohesive, playable campaign mode.
2. Review `IDEAS.md` for extended concepts (multiplayer networking optimizations, dedicated UI aesthetic overhauls, gyro controls).
