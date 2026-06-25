#!/bin/bash
echo "0.1.4" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.4] - 2026-06-24
- Implemented C++ physics engine toggle to blend Direct Input Torque mechanics and SMB World-Tilt Gravity.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
