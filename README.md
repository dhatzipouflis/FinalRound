# FinalRound

## Local development
- `dapr init --slim`
- `chmod +x scripts/dev/*.sh start-local-dev.command stop-local-dev.command`
- `./scripts/dev/start-infra.sh`
- run one by one with scripts or `dapr run -f .`

## Full docker stack
- `docker compose -p finalround -f infra/docker-compose.yml up --build`
- stop without removing containers: `docker compose -p finalround -f infra/docker-compose.yml stop`

## Persistence
- local infra uses named Docker volumes, so MySQL/Redis data remains after stop/start
- full docker stack also uses named Docker volumes, so data remains after stop/start
