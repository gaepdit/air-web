using AirWeb.AppServices.Core.AutoMapper;
using AutoMapper;

namespace AppServicesCoreTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        new MapperConfiguration(configure: configuration => configuration.AddProfile(new AutoMapperProfile()))
            .AssertConfigurationIsValid();
    }
}
