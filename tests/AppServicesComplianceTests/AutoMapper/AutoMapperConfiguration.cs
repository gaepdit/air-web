namespace AppServicesTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        Setup.MapperConfiguration!.AssertConfigurationIsValid();
    }
}
