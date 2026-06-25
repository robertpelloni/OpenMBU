#!/bin/bash
echo "0.1.5" > VERSION.md
cat << 'LOG' > CHANGELOG_NEW.md
## [0.1.5] - 2026-06-24
- Executed Repository Synchronization & Intelligent Merge. Fast-forwarded and reconciled AI feature branches.

LOG
cat CHANGELOG.md >> CHANGELOG_NEW.md
mv CHANGELOG_NEW.md CHANGELOG.md
