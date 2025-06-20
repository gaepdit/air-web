using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;

namespace AppServicesTests.WorkEntries.Search;

public class WorkEntryFilterTests
{
    private readonly WorkEntrySearchDto _baseSpec = new() { DeleteStatus = DeleteStatus.All };

    [Test]
    public void ResponsibleStaff_Match()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var responsibleStaffId = workEntries[0].ResponsibleStaff!.Id;
        var spec = _baseSpec with { ResponsibleStaff = responsibleStaffId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == responsibleStaffId);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ResponsibleStaff_NoMatch()
    {
        // Arrange
        var spec = _baseSpec with { ResponsibleStaff = Guid.Empty.ToString() };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Match()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var officeId = workEntries
            .First(entry => entry.ResponsibleStaff is { Office: not null }).ResponsibleStaff!.Office!.Id;

        var spec = _baseSpec with { Office = officeId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is { ResponsibleStaff.Office: not null } &&
            entry.ResponsibleStaff.Office.Id == officeId);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_NoMatch()
    {
        // Arrange
        var spec = _baseSpec with { Office = Guid.Empty };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void EventDate_Start()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var eventDate = (workEntries.First(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = _baseSpec with { EventDateFrom = eventDate, Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification notification &&
            notification.ReceivedDate >= eventDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EventDate_End()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var eventDate = (workEntries.First(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = _baseSpec with { EventDateTo = eventDate, Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification notification &&
            notification.ReceivedDate <= eventDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EventDate_StartAndEnd()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var eventDate = (workEntries.First(entry =>
            entry is { WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = _baseSpec with
        {
            EventDateTo = eventDate, EventDateFrom = eventDate, Include = [WorkTypeSearch.Notification]
        };

        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification notification &&
            notification.ReceivedDate == eventDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_Start()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var closedDate = workEntries.First(entry => entry is { ClosedDate: not null }).ClosedDate;

        var spec = _baseSpec with { ClosedDateFrom = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => entry.ClosedDate >= closedDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_End()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var closedDate = workEntries.First(entry => entry is { ClosedDate: not null }).ClosedDate;

        var spec = _baseSpec with { ClosedDateTo = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => entry.ClosedDate <= closedDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_StartAndEnd()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var closedDate = workEntries.First(entry => entry is { ClosedDate: not null }).ClosedDate;

        var spec = _baseSpec with
        {
            ClosedDateFrom = closedDate, ClosedDateTo = closedDate
        };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate == closedDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
