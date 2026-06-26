#!/bin/bash
echo "0.1.8" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.8] - 2026-06-24
- Fully integrated Obstacle Prototypes and Minigame hooks.
- Created `obstacle_course.mis` prototyping Bumpers, Switches, Gates, and Seesaws.
- Updated `monkey_target.mis` to accurately bind to the Party Game Framework.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
