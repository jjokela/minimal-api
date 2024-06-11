using Microsoft.EntityFrameworkCore;
using minimal_api_test.Context;
using minimal_api_test.Entity;

namespace minimal_api_test.Repository;

public interface ITodoRepository
{
    void Seed();
    Task<List<Todo>> GetTodosAsync();
    Task CreateTodoAsync(Todo todo);
    ValueTask<Todo?> GetTodoAsync(int id);
    Task<Todo?> UpdateTodoAsync(Todo inputTodo);
    Task<bool> DeleteTodoAsync(int id);
}

public class TodoRepository(TodoDb context) : ITodoRepository
{
    public async Task<List<Todo>> GetTodosAsync()
    {
        return await context.Todos.ToListAsync();
    }

    public async Task CreateTodoAsync(Todo todo)
    {
        context.Todos.Add(todo);
        await context.SaveChangesAsync();
    }

    public async ValueTask<Todo?> GetTodoAsync(int id)
    {
        return await context.Todos.FindAsync(id);
    }

    public async Task<Todo?> UpdateTodoAsync(Todo inputTodo)
    {
        var todo = await context.Todos.FindAsync(inputTodo.Id);

        if (todo == null)
        {
            return null;
        }

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;
        await context.SaveChangesAsync();

        return todo;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await context.Todos.FindAsync(id);

        if (todo == null)
        {
            return false;
        }

        context.Todos.Remove(todo);
        await context.SaveChangesAsync();

        return true;
    }

    public void Seed()
    {
        context.Database.EnsureCreated();

        if (context.Todos.Any())
        {
            return;
        }

        var todos = new[]
        {
            new Todo{Id=1, Name = "Tee sitä", IsComplete = false},
            new Todo{Id=2, Name = "Tee tätä", IsComplete = false},
        };

        context.Todos.AddRange(todos);
        context.SaveChanges();
    }
}