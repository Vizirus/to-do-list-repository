using DataLayer.DataLayer.ContextData;
using Microsoft.EntityFrameworkCore;

namespace BusinessTest.TestInfrastructure;

internal static class InMemoryDb
{
    public static DbContextOptions<ToDoListAppDbContext> CreateOptions(string databaseName)
    {
        return new DbContextOptionsBuilder<ToDoListAppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .EnableSensitiveDataLogging()
            .Options;
    }

    public static ToDoListAppDbContext CreateContext(DbContextOptions<ToDoListAppDbContext> options)
    {
        var context = new ToDoListAppDbContext(options);
        _ = context.Database.EnsureCreated();
        return context;
    }
}

