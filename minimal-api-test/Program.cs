using Microsoft.EntityFrameworkCore;
using minimal_api_test.Context;
using minimal_api_test.Endpoints;
using minimal_api_test.Repository;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);
ConfigureLogging(builder.Logging);

var app = builder.Build();

SeedDatabase(app);

TodoEndpoints.Register(app);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
    services.AddDatabaseDeveloperPageExceptionFilter();
    services.AddTransient<DbContext, TodoDb>();
    services.AddTransient<ITodoRepository, TodoRepository>();
}

void ConfigureLogging(ILoggingBuilder logging)
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
}

void SeedDatabase(WebApplication application)
{
    using var scope = application.Services.CreateScope();
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