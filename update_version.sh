#!/bin/bash
echo "0.1.12" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.12] - 2026-06-25
- Implemented script logic for SMB Conveyor Belts via continuous `applyImpulse` on tick.
- Refined the Banana Blitz jump mechanic to utilize a customizable `$Game::JumpForce` parameter.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
