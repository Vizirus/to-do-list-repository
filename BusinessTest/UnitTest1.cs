using BusinessTest.TestInfrastructure;

namespace BusinessTest;

public class AutoMapperConfigurationCreationTests
{
    [Fact]
    public void MapperFactoryCreatesMapper()
    {
        var mapper = MapperFactory.CreateBusinessMapper();
        Assert.NotNull(mapper);
    }
}
