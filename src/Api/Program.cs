using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using IBM.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string from environment or appsettings
var cs = builder.Configuration.GetConnectionString("Default")
         ?? builder.Configuration["ConnectionStrings:Default"]
         ?? "Server=localhost:50000;Database=appdb;User ID=db2inst1;Password=db2inst1;persist security info=true;";

// EF Core with DB2 using basic configuration
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseDb2(cs, b => { }));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


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
