using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.TestData.SampleData;

namespace AppServicesCoreTests.Staff;

public class StaffDtoTests
{
    [Test]
    public void DisplayName_TrimAll_TrimsItems()
    {
        var staffSearchDto = new StaffSearchDto
        {
            Name = " abc ",
            Email = " def ",
        };

        var result = staffSearchDto.TrimAll();

        using var scope = new AssertionScope();
        result.Name.Should().Be("abc");
        result.Email.Should().Be("def");
    }

    private static StaffViewDto ValidStaffView => new()
    {
        Id = Guid.Empty.ToString(),
        FamilyName = string.Empty,
        GivenName = string.Empty,
    };

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = ValidStaffView with { GivenName = givenName, FamilyName = familyName };
        staffViewDto.Name.Should().Be(expected);
    }

    [TestCase("abc", "def", "def, abc")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void SortableFullName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = ValidStaffView with { GivenName = givenName, FamilyName = familyName };
        staffViewDto.SortableFullName.Should().Be(expected);
    }

    [Test]
    public void AsUpdateDto_ExpectedBehavior()
    {
        var staffViewDto = ValidStaffView with
        {
            Id = Guid.NewGuid().ToString(),
            Active = true,
            PhoneNumber = SampleText.ValidPhoneNumber,
            Office = new OfficeViewDto { Id = Guid.NewGuid(), Name = SampleText.ValidName, Active = true },
        };

        var result = staffViewDto.AsUpdateDto();

        using var scope = new AssertionScope();
        result.Active.Should().BeTrue();
        result.PhoneNumber.Should().Be(staffViewDto.PhoneNumber);
        result.OfficeId.Should().Be(staffViewDto.Office.Id);
    }
}
