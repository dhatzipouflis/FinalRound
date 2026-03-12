#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-tournament \
  --app-port 5002 \
  --dapr-http-port 3512 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/services/FinalRound.Tournament.Api --urls "http://localhost:5002"
