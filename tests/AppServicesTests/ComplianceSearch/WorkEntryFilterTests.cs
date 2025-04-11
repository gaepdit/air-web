using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;

namespace AppServicesTests.ComplianceSearch;

public class WorkEntryFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto();
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry => !entry.IsDeleted);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_All_ReturnsAll()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeleteStatus = DeleteStatus.All };
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.ToList();

        // Act
        var result = expected.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_Deleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeleteStatus = DeleteStatus.Deleted };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry => entry.IsDeleted);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_Yes()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Closed = ClosedOpenAny.Closed };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry =>
            entry is { IsDeleted: false, IsClosed: true });

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_No()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Closed = ClosedOpenAny.Open };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry =>
            entry is { IsDeleted: false, IsClosed: false });

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_Single_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry =>
            entry is { IsDeleted: false, WorkEntryType: WorkEntryType.Notification });

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_Multiple_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkTypeSearch.Acc, WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries.Where(entry =>
            entry is
            {
                IsDeleted: false,
                WorkEntryType: WorkEntryType.AnnualComplianceCertification or WorkEntryType.Notification,
            });

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_MatchAll()
    {
        // Arrange
        var spec = new WorkEntrySearchDto
        {
            Include =
            [
                WorkTypeSearch.Acc, WorkTypeSearch.Inspection, WorkTypeSearch.Rmp, WorkTypeSearch.Report,
                WorkTypeSearch.Str, WorkTypeSearch.Notification, WorkTypeSearch.PermitRevocation,
            ],
            DeleteStatus = DeleteStatus.All,
        };

        var expression = WorkEntryFilters.SearchPredicate(spec);

        var workEntries = WorkEntryData.GetData.ToList();
        var expected = workEntries;

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch_WithHyphen()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var facilityId = workEntries.First(entry => !entry.IsDeleted).FacilityId;

        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => !entry.IsDeleted && entry.FacilityId == facilityId);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithHyphen()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var facilityId = workEntries[0].FacilityId[..5];

        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => !entry.IsDeleted && entry.FacilityId.Contains(facilityId));

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch_WithoutHyphen()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var facilityId = workEntries.First(entry => !entry.IsDeleted).FacilityId;

        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId.Trim('-') };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => !entry.IsDeleted && entry.FacilityId == facilityId);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithoutHyphen()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var facilityId = workEntries[0].FacilityId[..5];

        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId.Trim('-') };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => !entry.IsDeleted && entry.FacilityId.Contains(facilityId));

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_NoMatch()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { PartialFacilityId = "99999" };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void ResponsibleStaff_Match()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var responsibleStaffId = workEntries[0].ResponsibleStaff!.Id;
        var spec = new WorkEntrySearchDto { ResponsibleStaff = responsibleStaffId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == responsibleStaffId && !entry.IsDeleted);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ResponsibleStaff_NoMatch()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { ResponsibleStaff = Guid.Empty.ToString() };
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

        var spec = new WorkEntrySearchDto { Office = officeId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is { IsDeleted: false, ResponsibleStaff.Office: not null } &&
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
        var spec = new WorkEntrySearchDto { Office = Guid.Empty };
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
        var eventDate = (workEntries.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = new WorkEntrySearchDto { EventDateFrom = eventDate, Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
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
        var eventDate = (workEntries.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = new WorkEntrySearchDto { EventDateTo = eventDate, Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
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
        var eventDate = (workEntries.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;

        var spec = new WorkEntrySearchDto
            { EventDateTo = eventDate, EventDateFrom = eventDate, Include = [WorkTypeSearch.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
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
        var closedDate = workEntries.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;

        var spec = new WorkEntrySearchDto { ClosedDateFrom = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate >= closedDate);

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
        var closedDate = workEntries.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;

        var spec = new WorkEntrySearchDto { ClosedDateTo = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate <= closedDate);

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
        var closedDate = workEntries.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;

        var spec = new WorkEntrySearchDto
            { ClosedDateFrom = closedDate, ClosedDateTo = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate == closedDate);

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_Match()
    {
        // Arrange
        var workEntries = WorkEntryData.GetData.ToList();
        var note = workEntries.First(entry => !string.IsNullOrWhiteSpace(entry.Notes)).Notes![..3];

        var spec = new WorkEntrySearchDto { Notes = note };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = workEntries.Where(entry => entry.Notes != null && entry.Notes.Contains(note));

        // Act
        var result = workEntries.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_NoMatch()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Notes = Guid.NewGuid().ToString() };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }
}
