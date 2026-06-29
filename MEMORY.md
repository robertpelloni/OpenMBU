# MEMORY
## Architectural Observations
- **Engine:** Torque 3D / GarageGames derivative (Marble Blast engine).
- **Language:** C++ (engine) and TorqueScript (game logic).
- **Current State:** OpenMBU codebase focused on porting Xbox 360 MBU features to modern PC environments.

## Codebase Traits
- The game uses TorqueScript (`main.cs`, `marble/client`, `marble/server`) heavily for game flow, UI, and logic.
- Engine features are implemented in C++ and exposed to script.
- Submodules are tracked in `SUBMODULE_INVENTORY.md`.

## Design Preferences
- We prefer moving hardcoded values out of application logic and into script variables or single files (e.g. VERSION.md).
- Keep UI distinct and interactive.

## Physics Overhaul (World-Tilt vs Torque Physics)
- **Torque Physics:** The default Marble Blast engine uses direct input forces (torque) applied to the marble for rolling (`mv *= mDirectInputBlend`).
- **World-Tilt Gravity:** To emulate Super Monkey Ball, a global gravity tilting mechanism (`updateTiltGravity()`) is introduced. By dynamically shifting the `setGravityDir` based on user input, the world tilts around the ball.
- **Modularity:** To ensure compatibility, `mDirectInputBlend` (C++) was exposed. This allows developers to seamlessly toggle between the classic Torque physics (`mDirectInputBlend = 1.0`, gravity fixed) and the SMB Tilt physics (`mDirectInputBlend = 0.0`, gravity variable), or even blend both styles. Configuration lives in `gameParams.cs` (`$Game::UseWorldTilt`).
