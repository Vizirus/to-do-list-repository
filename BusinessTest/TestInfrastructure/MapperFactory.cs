using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using WebApi.BusinessLayer;

namespace BusinessTest.TestInfrastructure;

internal static class MapperFactory
{
    public static IMapper CreateBusinessMapper()
    {
        var expression = new MapperConfigurationExpression();
        expression.AddProfile(new BusinessLayerProfile());

        var config = new AutoMapper.MapperConfiguration(expression, NullLoggerFactory.Instance);
        return config.CreateMapper();
    }
}
