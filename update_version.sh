#!/bin/bash
echo "0.1.9" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.9] - 2026-06-24
- Refined Bumper physics to manually apply outward collision impulses.
- Extended Switch logic to universally support Platform, Door, and Spawn action types.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
