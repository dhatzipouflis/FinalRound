#!/bin/zsh
set +e

REPO_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$REPO_DIR"

echo "Stopping FinalRound local development..."

# Stop multi-app dapr template if running
dapr stop -f . >/dev/null 2>&1

# Stop explicit local app processes
pkill -f "FinalRound.Edge.Api" >/dev/null 2>&1
pkill -f "FinalRound.Tournament.Api" >/dev/null 2>&1
pkill -f "FinalRound.Identity.Api" >/dev/null 2>&1
pkill -f "FinalRound.Match.Api" >/dev/null 2>&1
pkill -f "FinalRound.Ranking.Api" >/dev/null 2>&1
pkill -f "FinalRound.Notification.Api" >/dev/null 2>&1

# Stop dapr sidecars started individually
pkill -f "dapr run.*finalround-edge" >/dev/null 2>&1
pkill -f "dapr run.*finalround-tournament" >/dev/null 2>&1
pkill -f "dapr run.*finalround-identity" >/dev/null 2>&1
pkill -f "dapr run.*finalround-match" >/dev/null 2>&1
pkill -f "dapr run.*finalround-ranking" >/dev/null 2>&1
pkill -f "dapr run.*finalround-notification" >/dev/null 2>&1

# Stop infra but keep data/containers
docker compose -f ./infra/local-dev/docker-compose.infra.yml stop >/dev/null 2>&1

echo "Stopped."