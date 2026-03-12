#!/bin/zsh
set -e
REPO_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$REPO_DIR"
chmod +x ./scripts/dev/*.sh
./scripts/dev/start-infra.sh
osascript <<OSA
 tell application "Terminal"
   activate
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-identity.sh"
   delay 1
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-tournament.sh"
   delay 1
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-match.sh"
   delay 1
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-ranking.sh"
   delay 1
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-notification.sh"
   delay 1
   do script "cd \"$REPO_DIR\" && ./scripts/dev/run-edge.sh"
 end tell
OSA
