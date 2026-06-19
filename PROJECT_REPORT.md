# OpenMBU: Project Architecture and Implementation Report

## Executive Summary
This report summarizes the architectural decisions, development challenges, and implemented solutions during the recent initiative to integrate Super Monkey Ball-style features into the OpenMBU engine. The goal was to establish a hybrid, ultimate marble/ball-rolling game utilizing the Torque Game Engine.

## Project Architecture
The OpenMBU project is built upon the **Torque Game Engine (TGE) / Torque 3D**.
- **Engine Core (C++):** Handles rendering, collision detection, basic networking, file I/O, and exposing engine functions/objects (like `MarbleData`, `ItemData`) to the scripting layer.
- **Scripting Layer (TorqueScript):** The vast majority of game logic, UI definition (`.gui` files), player input mapping, and mission handling are scripted in TorqueScript (`.cs` files) located in `game/marble/server/scripts/` and `game/marble/client/scripts/`.
- **UI System:** Client-side GUI is heavily data-driven through hierarchical profile definitions (e.g., `GuiBitmapCtrl`).

## Implemented Features
1. **Global Documentation Framework:** Established `VISION.md`, `ROADMAP.md`, `MEMORY.md`, and other foundational guidelines.
2. **Banana Collectables:** Developed `banana.cs` as a custom TorqueScript `ItemData` subclass to hook into `onPickup` events for score and extra life logic.
3. **World-Tilt Gravity System:** Implemented a prototype `tilt_gravity.cs` script to manipulate the global physics gravity vector, emulating SMB world-tilting.
4. **Native Jump Mechanic:** Bound input trigger `#2` inside `MarbleData::onTrigger` in the engine's C++ / Script hybrid layer to apply an upward impulse (`applyImpulse`) simulating Banana Blitz mechanics.
5. **UI Expansion:** Duplicated the client-side `GemBox` to create a `BananaBox` in `playGui.gui` and wired server-to-client commands to update the UI on item pickup.

## Development Challenges & Solutions

### 1. C++ Engine Compilation & Dependencies
- **Challenge:** Compiling the Torque engine on a modern Linux CI/sandbox environment failed due to missing, older dependencies (`nasm` for assembly optimization, `libsdl1.2-dev` for legacy windowing, and `pulseaudio` / OpenAL headers). Furthermore, the JSONCPP submodule threw namespace errors (`std::scoped_ptr` instead of `std::shared_ptr`).
- **Solution:** Provisioned the environment with `sudo apt-get install -y nasm libsdl1.2-dev`. Replaced deprecated smart pointers via script patches and manually fixed array `Offset` macros in `tsShapeConstruct.cpp` that failed to compile on modern GCC.

### 2. Headless Server Crashes (Audio Initialization)
- **Challenge:** Executing the built `./game/MBUltra` binary resulted in immediate segmentation faults and `AssertFatal` crashes within `sfxALDevice.cpp`. The OpenAL initialization routines assumed an audio device was present and aggressively asserted if context creation failed, making automated headless testing impossible.
- **Solution:** Refactored the device initialization in `sfxALDevice.cpp` to gracefully catch the failure and return an error message to the console instead of triggering a hard `AssertFatal` crash. We also fixed broken header definitions inside the engine's embedded OpenAL linux bindings.

## Areas for Future Optimization & Refactoring
Based on codebase analysis and `TODO` comments scattered throughout the project, the following areas require optimization:
1. **Client-side Audio (`playAudio` limitations):** As noted in `marble.cs`, `playAudio` does not reliably work on the client side in OpenMBU. This requires a deeper C++ networking pass to ensure 3D sounds (like jumps and pickups) are correctly ghosted to remote clients.
2. **Gem/Banana Spawning Logic:** `game.cs` explicitly notes that "currently some gems don't spawn when they should." The dynamic object spawning cycle during mission load needs refactoring to guarantee all scripted collectables initialize predictably.
3. **ScopeAlways Network Optimization:** Several object classes (e.g., Projectiles, HoverVehicles, Particle Emitters) have pending `TODO: ScopeAlways?` comments. Flagging static or highly visible objects as `ScopeAlways` instead of relying on the spatial ghosting manager could reduce CPU overhead and visual pop-in during fast-paced multiplayer sessions.
