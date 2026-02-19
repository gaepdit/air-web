namespace AppServicesTests.Core.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        Setup.MapperConfiguration!.AssertConfigurationIsValid();
    }
}
