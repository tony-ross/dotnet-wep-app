# .NET 8 Web API with IBM DB2 - Dev Container Setup

A development environment featuring .NET 8 Web API, Entity Framework Core with IBM DB2, and VS Code Dev Containers.

## Features

- **.NET 8 Web API** with minimal APIs and Swagger/OpenAPI
- **Entity Framework Core 8** with IBM DB2 support
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
   # Start IBM DB2 database
   docker compose up db -d

   # Create database table (manual step for now)
   # See Database Setup section below

   # Start API with hot reload
   cd src/Api
   dotnet watch run
   ```

### Option 2: Docker Compose Only

```bash
# Start everything
docker compose up --build

# Access API
# API: http://localhost:5187/swagger
# IBM DB2: localhost:50000
```

## Database Setup

Since IBM DB2 requires manual table creation, run this SQL command to create the Todos table:

```sql
CREATE TABLE Todos (
    Id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    IsDone SMALLINT DEFAULT 0
);
```

Execute this in the DB2 container:

```bash
docker exec -it dotnet-wep-app-db-1 bash -c "su - db2inst1 -c \"db2 connect to appdb; db2 'CREATE TABLE Todos (Id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY, Title VARCHAR(255) NOT NULL, IsDone SMALLINT DEFAULT 0)'; db2 disconnect appdb\""
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
# For IBM DB2 schema changes, use direct SQL
# Example: Add new column
# docker exec -it dotnet-wep-app-db-1 bash -c "su - db2inst1 -c \"db2 connect to appdb; db2 'ALTER TABLE Todos ADD COLUMN Description VARCHAR(500)'; db2 disconnect appdb\""
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
  -d '{"title": "Learn IBM DB2 with EF Core", "isDone": false}'

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
├── .gitignore            # Git ignore rules
├── CLAUDE.md             # Claude Code instructions
└── README.md
```

## Configuration

### Connection Strings

**Docker Compose (Default)**

```
Server=db:50000;Database=appdb;User ID=db2inst1;Password=db2inst1;persist security info=true;
```

**Local Development**

```
Server=localhost:50000;Database=appdb;User ID=db2inst1;Password=db2inst1;persist security info=true;
```

### Environment Variables

| Variable                 | Description  | Default             |
| ------------------------ | ------------ | ------------------- |
| `ASPNETCORE_ENVIRONMENT` | Environment  | Development         |
| `ASPNETCORE_URLS`        | API URL      | http://0.0.0.0:5187 |
| `DB2_USER`               | DB2 username | db2inst1            |
| `DB2_PASSWORD`           | DB2 password | db2inst1            |

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

3. **IBM EntityFrameworkCore issues**

   ```bash
   # Check package versions
   cd src/Api
   dotnet list package

   # Restore packages
   dotnet restore
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
4. Test with `dotnet build` and manual testing
5. Create pull request

## License

MIT License - feel free to use this setup for your own projects.
