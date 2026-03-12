#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-ranking \
  --app-port 5005 \
  --dapr-http-port 3505 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/services/FinalRound.Ranking.Api --urls "http://localhost:5005"
