# Session Handoff Document

## Current Status
- Finished adding dynamic conveyor belts via `smb_conveyors.cs`. These rely on rapid triggering (`onTickTrigger`) to continuously push the marble.
- Jump mechanics within `marble.cs` have been refactored for tunable impact via `$Game::JumpForce`.
- Expanded `PartyFramework` (`party_framework.cs`) with new lifecycle hooks (`onClientLeaveGame`, `onPlayerDeath`, `onPlayerFinish`). Fixed a TorqueScript `isFunction` syntax issue in dynamic dispatch.
- Implemented `Monkey Fight` minigame (`smb_fight.cs`) featuring lives tracking, elimination handling, and a custom radius-based punch mechanic (`InitContainerRadiusSearch`).
- Implemented `Monkey Race` minigame (`smb_race.cs`) incorporating a lap counting system with checkpoint triggers.
- Implemented `Monkey Billiards` minigame (`smb_billiards.cs`), adding pocket trigger logic, cue ball scratch penalties, and physics-driven impulse shooting using the camera's forward vector.
- Submodules, logs, and Git hygiene have been strictly maintained.

## Next Steps for Successor Model
1. Complete remaining Polish on physics values (restitution, slip, airAcceleration) to match true SMB parity.
2. We need robust `.mis` file creation to string these obstacles, bosses, and minigames together into a cohesive campaign layer.
3. Review `IDEAS.md` for extended concepts (multiplayer networking optimizations, dedicated UI aesthetic overhauls, gyro controls).
