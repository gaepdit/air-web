using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.Domain.Core.Entities;

namespace AppServicesCoreTests.AutoMapper;

public class OfficeMapping
{
    [Test]
    public void OfficeViewMapping_IncludesCorrectProperties()
    {
        var item = new Office(Guid.NewGuid(), SampleText.ValidName);

        var result = Setup.Mapper!.Map<OfficeViewDto>(item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeUpdateMapping_IncludesCorrectProperties()
    {
        var item = new Office(Guid.NewGuid(), SampleText.ValidName);

        var result = Setup.Mapper!.Map<OfficeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }
}
