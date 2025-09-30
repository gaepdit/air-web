using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.Offices;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _item = new()
    {
        Id = Guid.NewGuid().ToString(),
        GivenName = SampleText.ValidName,
        FamilyName = SampleText.NewValidName,
        Email = SampleText.ValidEmail,
        PhoneNumber = SampleText.ValidPhoneNumber,
        Office = new Office(Guid.NewGuid(), SampleText.ValidName),
    };

    [Test]
    public void StaffViewMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto>(_item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(_item.Id);
        result.GivenName.Should().Be(_item.GivenName);
        result.FamilyName.Should().Be(_item.FamilyName);
        result.Email.Should().Be(_item.Email);
        result.PhoneNumber.Should().Be(_item.PhoneNumber);
        result.Office.Should().BeEquivalentTo(_item.Office);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void StaffSearchResultMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffSearchResultDto>(_item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(_item.Id);
        result.SortableFullName.Should().Be($"{_item.FamilyName}, {_item.GivenName}");
        result.Email.Should().Be(_item.Email);
        result.OfficeName.Should().Be(_item.Office!.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void NullStaffViewMappingWorks()
    {
        ApplicationUser? item = null;
        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto?>(item);
        result.Should().BeNull();
    }
}
