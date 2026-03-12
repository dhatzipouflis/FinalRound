#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
dapr run \
  --app-id finalround-notification \
  --app-port 5006 \
  --dapr-http-port 3506 \
  --resources-path ./infra/dapr/components \
  --placement-host-address ' ' \
  --scheduler-host-address ' ' \
  -- dotnet run --project src/services/FinalRound.Notification.Api --urls "http://localhost:5006"
