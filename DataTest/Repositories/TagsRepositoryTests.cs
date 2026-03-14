using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class TagsRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TagsRepositoryTests)));

        int tagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TagsRepository(context);
            var entity = new Tags { Name = "Test tag" };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            tagId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tags.FirstOrDefaultAsync(x => x.Id == tagId);
            Assert.NotNull(saved);
            Assert.Equal("Test tag", saved!.Name);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TagsRepositoryTests)));

        int tagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TagsRepository(context);
            var entity = new Tags { Name = "Before" };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            tagId = entity.Id;

            entity.Name = "After";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tags.FirstOrDefaultAsync(x => x.Id == tagId);
            Assert.NotNull(saved);
            Assert.Equal("After", saved!.Name);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TagsRepositoryTests)));

        int tagId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TagsRepository(context);
            var entity = new Tags { Name = "To delete" };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            tagId = entity.Id;

            var removed = await repo.DeleteByIdAsync(tagId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.tags.FirstOrDefaultAsync(x => x.Id == tagId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TagsRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new TagsRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

