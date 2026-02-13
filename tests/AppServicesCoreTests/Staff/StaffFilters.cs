using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.TestData.Identity;

namespace AppServicesCoreTests.Staff;

public class StaffFilters
{
    private static StaffSearchDto DefaultStaffSearch => new();

    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var expected = UserData.Users.Where(e => e.Active).ToList();

        var result = UserData.Users.AsQueryable().ApplyFilter(DefaultStaffSearch).ToList();

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = UserData.Users.First(e => e.Active).GivenName;
        var spec = DefaultStaffSearch with { Name = name };
        var expected = UserData.Users
            .Where(e => e.Active &&
                        (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                         string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = UserData.Users.AsQueryable().ApplyFilter(spec).ToList();

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = UserData.Users.First(e => e.Active).Email;
        var spec = DefaultStaffSearch with { Email = email };
        var expected = UserData.Users
            .Where(e => e.Active && e.Email == email);

        var result = UserData.Users.AsQueryable().ApplyFilter(spec).ToList();

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = UserData.Users[0].Office;
        var spec = DefaultStaffSearch with { Office = office!.Id, Status = SearchStaffStatus.All };
        var expected = UserData.Users
            .Where(e => e.Office!.Id == office.Id);

        var result = UserData.Users.AsQueryable().ApplyFilter(spec).ToList();

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.Inactive };
        var expected = UserData.Users.Where(e => !e.Active);

        var result = UserData.Users.AsQueryable().ApplyFilter(spec).ToList();

        result.Should().BeEquivalentTo(expected, options => options
            .Excluding(user => user.SecurityStamp));
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.All };
        var result = UserData.Users.AsQueryable().ApplyFilter(spec).ToList();
        result.Should().BeEquivalentTo(UserData.Users, options => options
            .Excluding(user => user.SecurityStamp));
    }
}
