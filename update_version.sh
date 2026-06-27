#!/bin/bash
echo "0.1.10" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.10] - 2026-06-25
- Developed and integrated the Monkey Bowling minigame.
- Created `bowling_alley.mis` and fleshed out `smb_bowling.cs` with aiming restriction, forward impulse throws, and a pin collision/tipping calculator.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
