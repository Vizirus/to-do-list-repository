using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class TaskStatusesRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskStatusesRepositoryTests)));

        int statusId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskStatusesRepository(context);
            var entity = new TaskStatuses { Name = "Test status" };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            statusId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskStatuses.FirstOrDefaultAsync(x => x.Id == statusId);
            Assert.NotNull(saved);
            Assert.Equal("Test status", saved!.Name);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskStatusesRepositoryTests)));

        int statusId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskStatusesRepository(context);
            var entity = new TaskStatuses { Name = "Before" };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            statusId = entity.Id;

            entity.Name = "After";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskStatuses.FirstOrDefaultAsync(x => x.Id == statusId);
            Assert.NotNull(saved);
            Assert.Equal("After", saved!.Name);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskStatusesRepositoryTests)));

        int statusId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskStatusesRepository(context);
            var entity = new TaskStatuses { Name = "To delete" };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            statusId = entity.Id;

            var removed = await repo.DeleteByIdAsync(statusId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskStatuses.FirstOrDefaultAsync(x => x.Id == statusId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskStatusesRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new TaskStatusesRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

