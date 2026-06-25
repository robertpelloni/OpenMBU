#!/bin/bash
echo "0.1.7" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.7] - 2026-06-24
- Implemented Multi-Stage Boss Framework for Story Mode.
- Created `ApeBoss` prototype entity showcasing AI loops and phase transitions.
- Integrated Boss UI using the `bottomPrint` GUI module.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
