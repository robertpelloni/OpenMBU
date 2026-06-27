## [0.1.11] - 2026-06-25
- Finalized Party Game Framework scripts for Billiards and Golf minigames.
- Integrated `serverCmdBilliardsStrike` and `serverCmdGolfPutt` physics impulses.
- Set up target/pocket triggers for score tracking.

## [0.1.10] - 2026-06-25
- Developed and integrated the Monkey Bowling minigame.
- Created `bowling_alley.mis` and fleshed out `smb_bowling.cs` with aiming restriction, forward impulse throws, and a pin collision/tipping calculator.

## [0.1.11] - 2026-06-24
- Fixed TorqueScript scoping bug where `PartyFramework::onPlayerJoin` and `onPlayerSpawn` had an invalid `%this` signature, causing minigame initialization data tracking to fail.

## [0.1.10] - 2026-06-24
- Implemented Monkey Bowling prototype utilizing the Party Game Framework.
- Scripted dynamic pin spawning and `SMBBowlingPin` tipping physics.
- Established `serverCmdBowlingThrow` to restrict horizontal aiming and force velocity application.

## [0.1.8] - 2026-06-24
- Fully integrated Obstacle Prototypes and Minigame hooks.
- Created `obstacle_course.mis` prototyping Bumpers, Switches, Gates, and Seesaws.
- Updated `monkey_target.mis` to accurately bind to the Party Game Framework.

## [0.1.7] - 2026-06-24
- Implemented Multi-Stage Boss Framework for Story Mode.
- Created `ApeBoss` prototype entity showcasing AI loops and phase transitions.
- Integrated Boss UI using the `bottomPrint` GUI module.

## [0.1.6] - 2026-06-24
- Implemented modular Party Game Framework.
- Refactored Monkey Target to use the new framework architecture.
- Scaffolded scripts for Golf, Billiards, and Bowling minigames.
- Integrated framework hooks directly into the server game loop.

## [0.1.5] - 2026-06-24
- Executed Repository Synchronization & Intelligent Merge. Fast-forwarded and reconciled AI feature branches.

## [0.1.4] - 2026-06-24
- Implemented C++ physics engine toggle to blend Direct Input Torque mechanics and SMB World-Tilt Gravity.

## [0.1.2] - 2026-06-23
- Synchronized and intelligently merged remote feature branches.

## [0.1.1] - 2026-06-23
- Synchronized and intelligently merged remote feature branches.

# CHANGELOG
## [0.1.0] - Initial Setup
- Created core documentation (VISION, MEMORY, DEPLOY, IDEAS, CHANGELOG, ROADMAP, TODO).
- Established versioning standard in VERSION.md.
