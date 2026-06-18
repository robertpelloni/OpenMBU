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
