using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class UserRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(UserRepositoryTests)));

        int userId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new UserRepository(context);
            var entity = new User
            {
                Username = "test-user",
                Email = "test@example.com",
                PasswordHash = "hash",
                CreatedDate = new DateTime(2026, 3, 14),
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            userId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.users.FirstOrDefaultAsync(x => x.Id == userId);
            Assert.NotNull(saved);
            Assert.Equal("test-user", saved!.Username);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(UserRepositoryTests)));

        int userId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new UserRepository(context);
            var entity = new User
            {
                Username = "before",
                Email = "before@example.com",
                PasswordHash = "hash",
                CreatedDate = new DateTime(2026, 3, 14),
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            userId = entity.Id;

            entity.Username = "after";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.users.FirstOrDefaultAsync(x => x.Id == userId);
            Assert.NotNull(saved);
            Assert.Equal("after", saved!.Username);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(UserRepositoryTests)));

        int userId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new UserRepository(context);
            var entity = new User
            {
                Username = "to-delete",
                Email = "delete@example.com",
                PasswordHash = "hash",
                CreatedDate = new DateTime(2026, 3, 14),
            };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            userId = entity.Id;

            var removed = await repo.DeleteByIdAsync(userId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.users.FirstOrDefaultAsync(x => x.Id == userId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(UserRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new UserRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

