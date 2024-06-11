using minimal_api_test.Entity;
using minimal_api_test.Repository;

namespace minimal_api_test.Endpoints;

public class TodoEndpoints
{
    public static void Register(WebApplication app)
    {
        var todosGroup = app.MapGroup("/todos");

        app.MapGet("/", () => "Hello World!");

        todosGroup.MapGet("/", GetTodos);
        todosGroup.MapPost("/", CreateTodo);
        todosGroup.MapGet("/{id}", GetTodo);
        todosGroup.MapPut("/{id}", UpdateTodo);
        todosGroup.MapDelete("/{id}", DeleteTodo);
    }

    public static async Task<IResult> GetTodos(ITodoRepository repo)
    {
        return TypedResults.Ok(await repo.GetTodosAsync());
    }

    public static async Task<IResult> CreateTodo(Todo todo, ITodoRepository repo)
    {
        await repo.CreateTodoAsync(todo);
        return TypedResults.Created($"/todos/{todo.Id}", todo);
    }

    public static async Task<IResult> GetTodo(int id, ITodoRepository repo)
    {
        return await repo.GetTodoAsync(id) is { } todo ? TypedResults.Ok((Todo?)todo) : TypedResults.NotFound();
    }

    public static async Task<IResult> UpdateTodo(Todo inputTodo, ITodoRepository repo)
    {
        return await repo.UpdateTodoAsync(inputTodo) is not null ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static async Task<IResult> DeleteTodo(int id, ITodoRepository repo)
    {
        return await repo.DeleteTodoAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}