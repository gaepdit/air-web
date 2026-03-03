using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;

namespace AppServicesTests.Core.StaffTests;

public class StaffFilters
{
    [Test]
    public void DefaultFilter_ReturnsOnlyActive()
    {
        // Arrange
        var spec = new StaffSearchDto();
        var expected = TestData.StaffData.Where(user => user.Active).ToList();

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        // Arrange
        var spec = new StaffSearchDto { Name = TestData.NameSearch, Status = SearchStaffStatus.All };

        var expected = TestData.StaffData.Where(user =>
            string.Equals(user.GivenName, TestData.NameSearch, StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals(user.FamilyName, TestData.NameSearch, StringComparison.CurrentCultureIgnoreCase));

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        // Arrange
        var spec = new StaffSearchDto { Email = TestData.EmailSearch, Status = SearchStaffStatus.All };

        var expected = TestData.StaffData.Where(user =>
            string.Equals(user.Email, TestData.EmailSearch, StringComparison.CurrentCultureIgnoreCase));

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        // Arrange
        var officeId = TestData.OfficeData.Single(office => office.Name == TestData.OfficeSearch).Id;
        var spec = new StaffSearchDto { Office = officeId, Status = SearchStaffStatus.All };

        var expected = TestData.StaffData.Where(user =>
            user.Office is not null && user.Office.Id == officeId);

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InactiveFilter_ReturnsOnlyInactive()
    {
        // Arrange
        var spec = new StaffSearchDto { Status = SearchStaffStatus.Inactive };
        var expected = TestData.StaffData.Where(user => !user.Active).ToList();

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        // Arrange
        var spec = new StaffSearchDto { Status = SearchStaffStatus.All };

        // Act
        var result = TestData.StaffData.AsQueryable().ApplyFilter(spec).ToList();

        //Assert
        result.Should().BeEquivalentTo(TestData.StaffData);
    }
}
