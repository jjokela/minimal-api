using Microsoft.EntityFrameworkCore;
using minimal_api_test.Entity;

namespace minimal_api_test.Context;

public class TodoDb(DbContextOptions<TodoDb> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
}