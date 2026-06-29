# Super Monkey Ball: Collectables System

## Architecture
The OpenMBU project expands upon native Marble Blast item triggers to introduce a comprehensive Banana collectable system akin to the *Super Monkey Ball* franchise.

## Features
- **Scoring & Lives:** Collecting bananas increments an internal counter (`%user.client.bananas`). Hitting a specified threshold automatically grants the player an extra life and resets the counter. Points are also awarded to the global score.
- **Configurability:** System metrics are cleanly exposed to level designers via `gameParams.cs`. You can adjust:
  - `$Game::Collectables::BananaValue` (Default: 1)
  - `$Game::Collectables::BananaScore` (Default: 10)
  - `$Game::Collectables::ExtraLifeThreshold` (Default: 100)
- **Modularity:** Banana triggers use native `ItemData` collisions, meaning they integrate seamlessly regardless of whether the engine is currently using the classic Torque torque-rolling physics or the SMB world-tilt toggle mode.

## Prototypes
A prototype mission (`banana_blitz.mis`) exists in `data/missions/smb_prototypes/` demonstrating a line of collectables leading to an end pad.

## UI Hooks
The system broadcasts to the client via `commandToClient(%user.client, 'SetBananaCount', %user.client.bananas)`. Ensure your GUI (`playGui.gui`) contains a mapped variable matching this client command to display the banana graphic/counter on-screen.
