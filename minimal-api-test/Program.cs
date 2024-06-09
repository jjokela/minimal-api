using Microsoft.EntityFrameworkCore;
using minimal_api_test.Context;
using minimal_api_test.Endpoints;
using minimal_api_test.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<DbContext, TodoDb>();
builder.Services.AddTransient<ITodoRepository, TodoRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var repo = services.GetRequiredService<ITodoRepository>();
        repo.Seed();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

var todosGroup = app.MapGroup("/todos");

app.MapGet("/", () => "Hello World!");

todosGroup.MapGet("/", TodoEndpoints.GetTodos);
todosGroup.MapPost("/", TodoEndpoints.CreateTodo);
todosGroup.MapGet("/{id}", TodoEndpoints.GetTodo);
todosGroup.MapPut("/{id}", TodoEndpoints.UpdateTodo);
todosGroup.MapDelete("/{id}", TodoEndpoints.DeleteTodo);

app.Run();
