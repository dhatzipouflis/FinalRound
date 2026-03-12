#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-identity \
  --app-port 5003 \
  --dapr-http-port 3503 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/services/FinalRound.Identity.Api --urls "http://localhost:5003"
