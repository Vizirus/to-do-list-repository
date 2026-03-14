using BusinessLayer.BusinessLayer.Services;
using BusinessTest.TestInfrastructure;
using DataLayer.DataLayer.ContextData;

namespace BusinessTest.Integration;

public class TagsServiceIntegrationTests
{
    [Fact]
    public async Task GetAllAsyncReturnsSeededTags()
    {
        var options = InMemoryDb.CreateOptions(DatabaseName.ForTest(nameof(TagsServiceIntegrationTests)));

        await using var context = InMemoryDb.CreateContext(options);
        var unitOfWork = new UnitOfWork(context);
        var mapper = MapperFactory.CreateBusinessMapper();

        var service = new TagsService(unitOfWork, mapper);
        var tags = await service.GetAllAsync();

        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
    }
}
