# FinalRound
## Overview

This project is a demo microservices backend built with .NET 10.

So far, the main goal has been to set up the core architecture and local development workflow.  
The system is organized around separate services, with an Edge API acting as the main entry point for client requests.

Service-to-service communication is handled through Dapr, so the services stay more decoupled and easier to evolve independently.

For local development, the project includes infrastructure setup for components such as PostgreSQL and Redis, along with scripts and Docker-based workflows to make running the system easier both in development and in a full containerized environment.

The current focus is on establishing the project structure, communication flow, and local environment, so new services and features can be added on top of a stable foundation.

## Technologies

- .NET 10 for the backend services
- Dapr for service-to-service communication
- PostgreSQL for persistence
- Redis for caching / infrastructure support
- Docker Compose for local containerized setup
- Shell/command scripts for local development workflow

## Architecture

The project follows a microservices approach, where each service is intended to have a clear responsibility.

The client communicates first with the Edge API, which acts as the entry point to the system.  
From there, requests can be routed to the appropriate internal services.

The communication between services is supported by Dapr, which helps keep the architecture cleaner and more modular.

In addition, the project includes local infrastructure and development scripts, so the full environment can be started either piece by piece for development or through Docker for a more complete local stack.

## Local development
- `dapr init --slim`
- `chmod +x scripts/dev/*.sh start-local-dev.command stop-local-dev.command`
- `./scripts/dev/start-infra.sh`
- run one by one with scripts or `dapr run -f .`

## Full docker stack
- `docker compose -p finalround -f infra/docker-compose.yml up --build`
- stop without removing containers: `docker compose -p finalround -f infra/docker-compose.yml stop`

## Persistence
- local infra uses named Docker volumes, so PostgreSQL/Redis data remains after stop/start
- full docker stack also uses named Docker volumes, so data remains after stop/start
