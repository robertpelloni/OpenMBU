# Super Monkey Ball Features & Mechanics Research

## Core Mechanics
- **World Tilting (All Games):** The primary control scheme. The player tilts the stage, and gravity moves the ball.
- **Fallout (All Games):** Falling off the stage results in losing a life or a time penalty.
- **Time Limit (All Games):** Stages must be completed within a strict time limit.
- **Goals (All Games):** Break the tape to complete a level. Multiple goals exist: Green (Normal), Red (Warp), Green/Blue (Special).
- **Bananas (All Games):** Collectibles scattered across stages. 100 Bananas = 1 Extra Life.
- **Jump (Banana Blitz, Banana Mania):** Allows the ball to jump over gaps and obstacles.
- **Dash / Spin Dash (Banana Rumble):** A quick burst of speed.
- **Balance Board (Step & Roll):** Physical shifting of weight to control the stage.

## Stage Elements & Obstacles
- **Bumpers:** Bounce the player away upon contact.
- **Conveyor Belts:** Move the player automatically in a specific direction.
- **Seesaws:** Platforms that tilt based on the player's weight/position.
- **Switches & Buttons:** Trigger events like moving platforms, opening doors, or changing the stage layout.
- **Moving Platforms:** Platforms that move back and forth, up and down, or along tracks.
- **Gears / Rotating Cylinders:** Rotating elements that require precise timing to navigate.
- **Warp Gates:** Teleport the player from one part of the stage to another.
- **Ice / Frictionless Surfaces:** Reduced control over the ball.
- **Sand / High Friction Surfaces:** Slower movement.
- **Springs / Launchers:** Propel the player into the air.

## Game Modes
- **Main Game (Story / Challenge Mode):** Sequential levels with increasing difficulty (Beginner, Advanced, Expert, Master).
- **Party Games / Minigames:**
  - **Monkey Target:** Roll down a ramp, deploy wings, glide, and land on targets for points. Collect bananas and use items in the air.
  - **Monkey Billiards:** Pool game using the monkey balls.
  - **Monkey Bowling:** Bowling game with various lane shapes and obstacles.
  - **Monkey Golf:** Mini-golf with the monkey balls.
  - **Monkey Fight:** Punch other monkeys off an arena using boxing gloves.
  - **Monkey Race:** Kart racing-style game with power-ups.
  - **Monkey Boat:** Kayak racing.
  - **Monkey Shot:** Rail shooter.
  - **Monkey Dogfight:** Aerial combat.
  - **Monkey Soccer:** Soccer game.
  - **Monkey Baseball:** Baseball game.
  - **Monkey Tennis:** Tennis game.
  - **Monkey Snowboard:** Snowboarding down a mountain.

## Re-implementation Strategy for OpenMBU
1. **Control Scheme Hybrid:** Offer both MBU's direct-torque rolling and SMB's world-tilting. We can achieve this by hooking into the physics engine and adjusting global gravity based on input, or rotating the entire interior object (harder but more authentic).
2. **Collectibles:** Implement a robust item system for Bananas, extending MBU's Gem system.
3. **Obstacles:** Map MBU's existing hazards (Tornadoes, Bumpers, Fans, Mines) to SMB equivalents and create new ones (Seesaws, Conveyors) using Torque's pathing and trigger systems.
4. **Minigame Framework:** Create separate game modes (`$Game::Mode = "Target";`) that load specific GUI overlays and alter ball physics (e.g., adding air control for Monkey Target).

## SMB Obstacles Implementation Details
We have implemented the following obstacles in TorqueScript:
*   **Bumpers (`smb_bumpers.cs`):** Utilizing a custom `MaterialProperty` (`SMBBumperMaterial`), we increased the default `force` value to `25` (compared to MBU's standard 15) to simulate the harsher knockback of Super Monkey Ball bumpers.
*   **Switches & Gates (`smb_switches.cs`):** A custom `StaticShape` that triggers a `playThread` animation when pressed, and iterates through a mapped `targetGroup` to activate `PathedInterior` gates.
*   **Warp Gates (`smb_warpgates.cs`):** A `TriggerData` object that safely intercepts the marble and uses `%obj.setPosition(%destPos, true)` to teleport the player to a target node.
*   **Elevators/Moving Platforms (`smb_platforms.cs`):** Wrappers around MBU's `PathedInterior` system providing continuous looping audio profiles for typical SMB constant-movement objects.
*   **Seesaws (`smb_seesaws.cs`):** A `RigidShapeData` element that offsets its `massCenter` below its visual origin (`0 0 -0.5`). This allows the Torque physics engine to naturally balance the object like a pendulum, creating a physics-based seesaw when the marble rolls across it.

## Minigame Prototypes
### Monkey Target
*   **Glider Mechanics (`smb_monkey_target.cs`):**
    *   Toggled via the server command `serverCmdToggleMonkeyTarget`.
    *   Once enabled, intercepting `triggerNum 1` (the blast/use item button) in `MarbleData::onTrigger` allows the player to deploy/retract their glider.
    *   Deploying the glider swaps the active datablock to `GliderMarble`, which drastically reduces gravity (20 -> 5) and increases `airAcceleration` (5 -> 25) to provide flight control.
    *   Deploying also gives a slight upward boost (`applyImpulse` of 2).
*   **Target Zones:**
    *   Implemented via `SMBTargetZoneTrigger`. Level designers can place Trigger bounds in the `.mis` file and define dynamic `points` attributes. Landing in these zones halts the player's momentum, awards the points, and safely resets the round to attempt again.
