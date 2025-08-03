# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

A .NET 8 Web API with DB2 using Entity Framework Core, configured for development in VS Code Dev Containers with Docker Compose for local development. The project follows minimal API patterns with clean architecture.

## Quick Commands

### Development Environment
- **Start database**: `docker compose up db -d`
- **Start API with hot reload**: `cd src/Api && dotnet watch run`
- **Dev Container**: Open VS Code → F1 → "Dev Containers: Reopen in Container"
- **Full Docker setup**: `docker compose up --build`

### Database Management (DB2)
- **Apply migrations**: `cd src/Api && dotnet ef database update`
- **Create migration**: `dotnet ef migrations add <Name>`
- **Remove migration**: `dotnet ef migrations remove`
- **Reset database**: `dotnet ef database drop --force`

### API Access
- **Swagger UI**: http://localhost:5187/swagger
- **Health check**: http://localhost:5187/swagger/v1/swagger.json

## Architecture

### Project Structure
```
src/Api/
├── Models/          # Data models (TodoItem.cs)
├── Data/            # EF Core context (AppDbContext.cs)
├── Program.cs       # Minimal API endpoints
└── appsettings*.json # Configuration
```

### Key Components
- **AppDbContext**: EF Core DbContext with PostgreSQL via Npgsql
- **TodoItem**: Simple model with Id, Title, IsDone properties
- **Minimal APIs**: RESTful endpoints for CRUD operations on todos
- **Swagger**: Auto-generated API documentation

### Technology Stack
- **.NET 8** with minimal APIs
- **Entity Framework Core 8** with DB2
- **IBM.EntityFrameworkCore**
- **Swashbuckle.AspNetCore** for Swagger
- **Docker & Docker Compose** for containerization
- **VS Code Dev Containers** for consistent development

## Configuration

### Connection Strings
- **Dev Container**: `Server=db:50000;Database=appdb;User ID=db2inst1;Password=db2inst1;persist security info=true;`
- **Local**: `Server=localhost:50000;Database=appdb;User ID=db2inst1;Password=db2inst1;persist security info=true;`

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development
- `ASPNETCORE_URLS`: http://0.0.0.0:5187
- `ConnectionStrings__Default`: PostgreSQL connection string

## Dev Container Setup

Located in `.devcontainer/` with:
- **Dockerfile**: .NET 8 SDK with non-root user
- **devcontainer.json**: VS Code extensions and settings
- **Extensions**: C#, CS Dev Kit, Docker, Test Explorer, EditorConfig

## Common Tasks

### Adding New Endpoints
1. Add model to `Models/`
2. Update `AppDbContext` with new `DbSet`
3. Create migration: `dotnet ef migrations add <Name>`
4. Add endpoints in `Program.cs`

### Testing
- **Test Explorer**: Available via VS Code extension
- **Manual testing**: Use Swagger UI or curl commands

### Troubleshooting
- **Port conflicts**: Check with `lsof -i :5187`
- **Database issues**: Check `docker compose logs db`
- **Migration problems**: Reset with `dotnet ef database drop --force`