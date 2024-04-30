using MyApp.AppServices.Staff.Dto;
using MyApp.Domain.Entities.Offices;
using MyApp.Domain.Identity;
using MyApp.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _item = new()
    {
        Id = Guid.NewGuid().ToString(),
        GivenName = TextData.ValidName,
        FamilyName = TextData.NewValidName,
        Email = TextData.ValidEmail,
        PhoneNumber = TextData.ValidPhoneNumber,
        Office = new Office(Guid.NewGuid(), TextData.ValidName),
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
