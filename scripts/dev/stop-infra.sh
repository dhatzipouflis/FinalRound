#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/../.."
docker compose -f ./infra/local-dev/docker-compose.infra.yml stop
