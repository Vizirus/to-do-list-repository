using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class ListsRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(ListsRepositoryTests)));

        int listId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new ListsRepository(context);
            var entity = new Lists
            {
                ListName = "Test list",
                CreatedByUser = 1,
                CreatedDate = new DateTime(2026, 3, 14),
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            listId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.lists.FirstOrDefaultAsync(x => x.Id == listId);
            Assert.NotNull(saved);
            Assert.Equal("Test list", saved!.ListName);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(ListsRepositoryTests)));

        int listId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new ListsRepository(context);
            var entity = new Lists { ListName = "Before", CreatedByUser = 1, CreatedDate = new DateTime(2026, 3, 14) };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            listId = entity.Id;

            entity.ListName = "After";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.lists.FirstOrDefaultAsync(x => x.Id == listId);
            Assert.NotNull(saved);
            Assert.Equal("After", saved!.ListName);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(ListsRepositoryTests)));

        int listId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new ListsRepository(context);
            var entity = new Lists { ListName = "To delete", CreatedByUser = 1, CreatedDate = new DateTime(2026, 3, 14) };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            listId = entity.Id;

            var removed = await repo.DeleteByIdAsync(listId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.lists.FirstOrDefaultAsync(x => x.Id == listId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(ListsRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new ListsRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

