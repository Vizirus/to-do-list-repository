using DataLayer.DataLayer.Entities;
using DataLayer.DataLayer.Repositories;
using DataTest.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataTest.Repositories;

public class TaskCommentsRepositoryTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddAsyncPersistsEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskCommentsRepositoryTests)));

        int commentId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskCommentsRepository(context);
            var entity = new TaskComments { TaskId = 1, UserId = 1, CommentText = "Hello", CreatedDate = new DateTime(2026, 3, 14) };

            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            commentId = entity.Id;
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskComments.FirstOrDefaultAsync(x => x.Id == commentId);
            Assert.NotNull(saved);
            Assert.Equal("Hello", saved!.CommentText);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdatePersistsChanges()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskCommentsRepositoryTests)));

        int commentId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskCommentsRepository(context);
            var entity = new TaskComments { TaskId = 1, UserId = 1, CommentText = "Before", CreatedDate = new DateTime(2026, 3, 14) };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            commentId = entity.Id;

            entity.CommentText = "After";
            repo.Update(entity);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskComments.FirstOrDefaultAsync(x => x.Id == commentId);
            Assert.NotNull(saved);
            Assert.Equal("After", saved!.CommentText);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncRemovesEntity()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskCommentsRepositoryTests)));

        int commentId;
        await using (var context = InMemoryDb.CreateContext(options))
        {
            var repo = new TaskCommentsRepository(context);
            var entity = new TaskComments { TaskId = 1, UserId = 1, CommentText = "To delete", CreatedDate = new DateTime(2026, 3, 14) };
            await repo.AddAsync(entity);
            _ = await context.SaveChangesAsync();
            commentId = entity.Id;

            var removed = await repo.DeleteByIdAsync(commentId);
            Assert.True(removed);
            _ = await context.SaveChangesAsync();
        }

        await using (var verify = InMemoryDb.CreateContext(options))
        {
            var saved = await verify.taskComments.FirstOrDefaultAsync(x => x.Id == commentId);
            Assert.Null(saved);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteByIdAsyncUnknownIdReturnsFalse()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TaskCommentsRepositoryTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var repo = new TaskCommentsRepository(context);

        var removed = await repo.DeleteByIdAsync(999999);

        Assert.False(removed);
    }
}

