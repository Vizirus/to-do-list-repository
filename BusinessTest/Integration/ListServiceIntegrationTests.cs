using BusinessLayer.BusinessLayer.Services;
using BusinessTest.TestInfrastructure;
using DataLayer.DataLayer.ContextData;

namespace BusinessTest.Integration;

public class ListServiceIntegrationTests
{
    [Fact]
    public async Task GetAllAsyncReturnsSeededLists()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(ListServiceIntegrationTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var unitOfWork = new UnitOfWork(context);
        var mapper = MapperFactory.CreateBusinessMapper();

        var service = new ListService(unitOfWork, mapper);
        var lists = await service.GetAllAsync();

        Assert.NotNull(lists);
        Assert.NotEmpty(lists);
    }
}
