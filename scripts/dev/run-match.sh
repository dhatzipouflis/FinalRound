#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-match \
  --app-port 5004 \
  --dapr-http-port 3504 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/services/FinalRound.Match.Api --urls "http://localhost:5004"
