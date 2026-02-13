using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Core.Entities;
using AirWeb.TestData.SampleData;

namespace AppServicesCoreTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _user = new()
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
        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto>(_user);

        using var scope = new AssertionScope();
        result.Id.Should().Be(_user.Id);
        result.GivenName.Should().Be(_user.GivenName);
        result.FamilyName.Should().Be(_user.FamilyName);
        result.Email.Should().Be(_user.Email);
        result.PhoneNumber.Should().Be(_user.PhoneNumber);
        result.Office.Should().BeEquivalentTo(_user.Office);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void StaffViewMapping_WithNullValues_Works()
    {
        ApplicationUser user = new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = string.Empty,
            FamilyName = string.Empty,
            Email = null,
            PhoneNumber = SampleText.ValidPhoneNumber,
            Office = null,
        };

        var result = AppServicesTestsSetup.Mapper!.Map<StaffViewDto>(user);

        using var scope = new AssertionScope();
        result.Id.Should().Be(user.Id);
        result.GivenName.Should().Be(user.GivenName);
        result.FamilyName.Should().Be(user.FamilyName);
        result.Email.Should().BeNull();
        result.PhoneNumber.Should().Be(user.PhoneNumber);
        result.Office.Should().BeNull();
        result.Active.Should().BeTrue();
    }

    [Test]
    public void StaffSearchResultMappingWorks()
    {
        var result = AppServicesTestsSetup.Mapper!.Map<StaffSearchResultDto>(_user);

        using var scope = new AssertionScope();
        result.Id.Should().Be(_user.Id);
        result.SortableFullName.Should().Be($"{_user.FamilyName}, {_user.GivenName}");
        result.Email.Should().Be(_user.Email);
        result.OfficeName.Should().Be(_user.Office!.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void StaffSearchResultMapping_WithNullProperties_Works()
    {
        ApplicationUser user = new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = string.Empty,
            FamilyName = string.Empty,
            Email = null,
            PhoneNumber = SampleText.ValidPhoneNumber,
            Office = null,
        };

        var result = AppServicesTestsSetup.Mapper!.Map<StaffSearchResultDto>(user);

        using var scope = new AssertionScope();
        result.Id.Should().Be(user.Id);
        result.SortableFullName.Should().Be(string.Empty);
        result.Email.Should().BeNull();
        result.OfficeName.Should().BeNull();
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
