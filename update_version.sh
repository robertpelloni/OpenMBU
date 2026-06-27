#!/bin/bash
echo "0.1.11" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.11] - 2026-06-25
- Finalized Party Game Framework scripts for Billiards and Golf minigames.
- Integrated `serverCmdBilliardsStrike` and `serverCmdGolfPutt` physics impulses.
- Set up target/pocket triggers for score tracking.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
