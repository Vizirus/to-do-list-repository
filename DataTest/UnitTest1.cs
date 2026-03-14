using DataLayer.DataLayer.ContextData;
using DataTest.TestInfrastructure;
using Moq;

namespace DataTest;

public class UnitOfWorkTests
{
    [Fact]
    public async System.Threading.Tasks.Task SaveAsyncCallsSaveChangesAsyncOnce()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(UnitOfWorkTests)));

        var mockContext = new Mock<ToDoListAppDbContext>(options);
        _ = mockContext
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var unitOfWork = new UnitOfWork(mockContext.Object);

        await unitOfWork.SaveAsync();

        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

