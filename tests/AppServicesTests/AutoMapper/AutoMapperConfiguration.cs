using AirWeb.AppServices.AutoMapper;
using AutoMapper;

namespace AppServicesTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        new MapperConfiguration(configure: configuration => configuration.AddProfile(new AutoMapperProfile()))
            .AssertConfigurationIsValid();
    }
}
