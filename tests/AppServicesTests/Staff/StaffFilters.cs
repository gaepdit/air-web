using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    private static StaffSearchDto DefaultStaffSearch => new();

    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var expected = UserData.GetData.Where(e => e.Active);

        var result = UserData.GetData.AsQueryable().ApplyFilter(DefaultStaffSearch);

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = UserData.GetData.First(e => e.Active).GivenName;
        var spec = DefaultStaffSearch with { Name = name };
        var expected = UserData.GetData
            .Where(e => e.Active &&
                        (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                         string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = UserData.GetData.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = UserData.GetData.First(e => e.Active).Email;
        var spec = DefaultStaffSearch with { Email = email };
        var expected = UserData.GetData
            .Where(e => e.Active && e.Email == email);

        var result = UserData.GetData.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = UserData.GetData.First().Office;
        var spec = DefaultStaffSearch with { Office = office!.Id, Status = SearchStaffStatus.All };
        var expected = UserData.GetData
            .Where(e => e.Office!.Id == office.Id);

        var result = UserData.GetData.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.Inactive };
        var expected = UserData.GetData.Where(e => !e.Active);

        var result = UserData.GetData.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.All };
        var result = UserData.GetData.AsQueryable().ApplyFilter(spec);
        result.Should().BeEquivalentTo(UserData.GetData, options => options
            .Excluding(user => user.SecurityStamp));
    }
}
