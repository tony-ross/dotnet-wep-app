using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string from environment or appsettings
var cs = builder.Configuration.GetConnectionString("Default")
         ?? builder.Configuration["ConnectionStrings:Default"]
         ?? "Host=localhost;Port=5432;Database=appdb;Username=app;Password=app";

// EF Core with Npgsql
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(cs));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply pending migrations at startup (optional for dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Minimal API endpoints
app.MapGet("/api/todos", async (AppDbContext db) =>
    Results.Ok(await db.Todos.OrderByDescending(t => t.Id).ToListAsync()));

app.MapGet("/api/todos/{id:int}", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.MapPost("/api/todos", async (TodoItem todo, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(todo.Title))
        return Results.BadRequest(new { error = "Title is required" });

    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/todos/{todo.Id}", todo);
});

app.MapPut("/api/todos/{id:int}", async (int id, TodoItem updated, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Title = updated.Title;
    todo.IsDone = updated.IsDone;
    await db.SaveChangesAsync();
    return Results.Ok(todo);
});

app.MapDelete("/api/todos/{id:int}", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
