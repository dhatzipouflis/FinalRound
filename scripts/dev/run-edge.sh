#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-edge \
  --app-port 5001 \
  --dapr-http-port 3500 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/edge/FinalRound.Edge.Api --urls "https://localhost:7001;http://localhost:5001"
