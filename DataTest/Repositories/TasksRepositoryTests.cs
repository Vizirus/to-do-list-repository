using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DataLayer.DataLayer.Entities.Task;

namespace DataTest.Repositories;

public class TasksRepositoryTests
{
    [Fact]
    public async Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TasksRepositoryTests)));

        int taskId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TasksRepository(context);
            var entity = new TaskEntity
            {
                ListId = 1,
                TaskName = "Test task",
                TaskDescription = "Desc",
                TaskStartDate = new DateTime(2026, 3, 14, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 15, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 1,
                AssigndUserId = 1,
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            Assert.NotNull(saved);
            Assert.Equal("Test task", saved!.TaskName);
        }
    }

    [Fact]
    public async Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TasksRepositoryTests)));

        int taskId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TasksRepository(context);
            var entity = new TaskEntity
            {
                ListId = 1,
                TaskName = "Before",
                TaskDescription = "Desc",
                TaskStartDate = new DateTime(2026, 3, 14, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 15, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 1,
                AssigndUserId = 1,
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskId = entity.Id;

            entity.TaskName = "After";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            Assert.NotNull(saved);
            Assert.Equal("After", saved!.TaskName);
        }
    }

    [Fact]
    public async Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TasksRepositoryTests)));

        int taskId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TasksRepository(context);
            var entity = new TaskEntity
            {
                ListId = 1,
                TaskName = "To delete",
                TaskDescription = "Desc",
                TaskStartDate = new DateTime(2026, 3, 14, 1, 1, 1, DateTimeKind.Utc),
                TaskFinishDate = new DateTime(2026, 3, 15, 1, 1, 1, DateTimeKind.Utc),
                StatusId = 1,
                AssigndUserId = 1,
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskId = entity.Id;

            var removed = await repo.DeleteByIdAsync(taskId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TasksRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new TasksRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

