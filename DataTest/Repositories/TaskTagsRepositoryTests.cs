using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class TaskTagsRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskTagsRepositoryTests)));

        int taskTagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskTagsRepository(context);
            var entity = new TaskTags { TaskId = 1, TagId = 1 };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskTagId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskTags.FirstOrDefaultAsync(x => x.Id == taskTagId);
            Assert.NotNull(saved);
            Assert.Equal(1, saved!.TaskId);
            Assert.Equal(1, saved.TagId);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskTagsRepositoryTests)));

        int taskTagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskTagsRepository(context);
            var entity = new TaskTags { TaskId = 1, TagId = 1 };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskTagId = entity.Id;

            entity.TagId = 2;
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskTags.FirstOrDefaultAsync(x => x.Id == taskTagId);
            Assert.NotNull(saved);
            Assert.Equal(2, saved!.TagId);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskTagsRepositoryTests)));

        int taskTagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskTagsRepository(context);
            var entity = new TaskTags { TaskId = 1, TagId = 1 };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            taskTagId = entity.Id;

            var removed = await repo.DeleteByIdAsync(taskTagId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskTags.FirstOrDefaultAsync(x => x.Id == taskTagId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskTagsRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new TaskTagsRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

