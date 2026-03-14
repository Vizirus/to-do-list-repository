using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class SharedListsRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(SharedListsRepositoryTests)));

        int sharedListId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new SharedListsRepository(context);
            var entity = new SharedLists { ToDoListId = 1, UserWhoAssignsIs = 1, AssignedUserId = 2 };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            sharedListId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.sharedLists.FirstOrDefaultAsync(x => x.Id == sharedListId);
            Assert.NotNull(saved);
            Assert.Equal(1, saved!.ToDoListId);
            Assert.Equal(2, saved.AssignedUserId);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(SharedListsRepositoryTests)));

        int sharedListId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new SharedListsRepository(context);
            var entity = new SharedLists { ToDoListId = 1, UserWhoAssignsIs = 1, AssignedUserId = 2 };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            sharedListId = entity.Id;

            entity.AssignedUserId = 3;
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.sharedLists.FirstOrDefaultAsync(x => x.Id == sharedListId);
            Assert.NotNull(saved);
            Assert.Equal(3, saved!.AssignedUserId);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(SharedListsRepositoryTests)));

        int sharedListId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new SharedListsRepository(context);
            var entity = new SharedLists { ToDoListId = 1, UserWhoAssignsIs = 1, AssignedUserId = 2 };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            sharedListId = entity.Id;

            var removed = await repo.DeleteByIdAsync(sharedListId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.sharedLists.FirstOrDefaultAsync(x => x.Id == sharedListId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(SharedListsRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new SharedListsRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

