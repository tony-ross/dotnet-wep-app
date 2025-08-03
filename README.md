# .NET 8 Web API with PostgreSQL - Dev Container Setup

A production-ready development environment featuring .NET 8 Web API, Entity Framework Core with PostgreSQL, and VS Code Dev Containers for consistent development across teams.

## Features

- **.NET 8 Web API** with minimal APIs and Swagger/OpenAPI
- **Entity Framework Core 8** with PostgreSQL support
- **VS Code Dev Containers** for consistent development environment
- **Docker Compose** for easy local development
- **Hot reload** support for rapid development
- **Migration support** with EF Core tools
- **Clean architecture** with separate models, data, and API layers

## Quick Start

### Option 1: Dev Container (Recommended)

1. **Prerequisites**

   - Docker Desktop
   - VS Code with Dev Containers extension
   - Git

2. **Setup**

   ```bash
   git clone <repository-url>
   cd dotnet-wep-app
   code .
   ```

3. **Open in Dev Container**

   - Press `F1` → "Dev Containers: Reopen in Container"
   - Wait for the container to build and initialize

4. **Start Development**

   ```bash
   # Start PostgreSQL database
   docker compose up db -d

   # Apply database migrations
   cd src/Api
   dotnet ef database update

   # Start API with hot reload
   dotnet watch run
   ```

### Option 2: Docker Compose Only

```bash
# Start everything
docker compose up --build

# Access API
# API: http://localhost:5187/swagger
# PostgreSQL: localhost:5432
```

## Development Workflow

### Daily Development

```bash
# Start development environment
docker compose up db -d
cd src/Api
dotnet watch run

# Access Swagger UI
open http://localhost:5187/swagger
```

### Database Changes

```bash
# Create new migration
cd src/Api
dotnet ef migrations add AddNewFeature

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### API Endpoints

| Method | Endpoint          | Description     |
| ------ | ----------------- | --------------- |
| GET    | `/api/todos`      | Get all todos   |
| GET    | `/api/todos/{id}` | Get todo by ID  |
| POST   | `/api/todos`      | Create new todo |
| PUT    | `/api/todos/{id}` | Update todo     |
| DELETE | `/api/todos/{id}` | Delete todo     |

### Example Usage

```bash
# Create a todo
curl -X POST http://localhost:5187/api/todos \
  -H "Content-Type: application/json" \
  -d '{"title": "Learn Dev Containers", "isDone": false}'

# Get all todos
curl http://localhost:5187/api/todos
```

## Project Structure

```
├── .devcontainer/          # Dev container configuration
│   ├── devcontainer.json
│   └── Dockerfile
├── src/
│   └── Api/               # .NET Web API project
│       ├── Models/        # Data models
│       ├── Data/          # EF Core context
│       ├── Program.cs     # API endpoints
│       └── appsettings*.json
├── docker-compose.yml     # Service orchestration
└── README.md
```

## Configuration

### Connection Strings

**Docker Compose (Default)**

```
Host=db;Port=5432;Database=appdb;Username=app;Password=app
```

**Local Development**

```
Host=localhost;Port=5432;Database=appdb;Username=app;Password=app
```

### Environment Variables

| Variable                     | Description           | Default             |
| ---------------------------- | --------------------- | ------------------- |
| `ASPNETCORE_ENVIRONMENT`     | Environment           | Development         |
| `ASPNETCORE_URLS`            | API URL               | http://0.0.0.0:5187 |
| `ConnectionStrings__Default` | PostgreSQL connection | See above           |

## Troubleshooting

### Common Issues

1. **Port already in use**

   ```bash
   # Check what's using the port
   lsof -i :5187

   # Change port in docker-compose.yml
   ports:
     - "5188:5187"  # Use 5188 instead
   ```

2. **Database connection issues**

   ```bash
   # Check database health
   docker compose ps
   docker compose logs db

   # Reset database
   docker compose down -v
   docker compose up --build
   ```

3. **Migration issues**
   ```bash
   # Reset migrations
   cd src/Api
   dotnet ef database drop --force
   dotnet ef migrations remove
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### Performance Tips

- **macOS**: Use Dev Containers to avoid bind mount performance issues
- **Windows**: Ensure Docker Desktop is using WSL2 backend
- **Linux**: Native performance, no special configuration needed

## Clean Up

```bash
# Stop all services
docker compose down

# Remove volumes (database data)
docker compose down -v

# Remove images
docker compose down --rmi all
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make changes in Dev Container
4. Test with `dotnet test` (when tests are added)
5. Create pull request

## License

MIT License - feel free to use this setup for your own projects.
