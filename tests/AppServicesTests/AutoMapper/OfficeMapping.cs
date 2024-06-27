using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.Domain.Entities.Offices;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.AutoMapper;

public class OfficeMapping
{
    [Test]
    public void OfficeViewMapping_IncludesCorrectProperties()
    {
        var item = new Office(Guid.NewGuid(), SampleText.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeViewDto>(item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeUpdateMapping_IncludesCorrectProperties()
    {
        var item = new Office(Guid.NewGuid(), SampleText.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }
}
