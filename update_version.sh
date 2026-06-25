#!/bin/bash
echo "0.1.6" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.6] - 2026-06-24
- Implemented modular Party Game Framework.
- Refactored Monkey Target to use the new framework architecture.
- Scaffolded scripts for Golf, Billiards, and Bowling minigames.
- Integrated framework hooks directly into the server game loop.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
