using AirWeb.AppServices.Compliance.Search;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Entities;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.ComplianceSearch;

public class WorkEntryFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto();
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.Where(entry => !entry.IsDeleted);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_All_ReturnsAll()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeleteStatus = DeleteStatus.All };
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_Deleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeleteStatus = DeleteStatus.Deleted };
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.Where(entry => entry.IsDeleted);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_Yes()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Closed = YesNoAny.Yes };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, IsClosed: true });

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_No()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Closed = YesNoAny.No };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, IsClosed: false });

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_NonCompliance_Single_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkEntryTypes.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, WorkEntryType: WorkEntryType.Notification });

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_Compliance_Single_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkEntryTypes.Acc] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, WorkEntryType: WorkEntryType.ComplianceEvent } &&
            ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.AnnualComplianceCertification);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_NonCompliance_Multiple_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkEntryTypes.Notification, WorkEntryTypes.PermitRevocation] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, WorkEntryType: WorkEntryType.Notification or WorkEntryType.PermitRevocation });

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_Compliance_Multiple_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkEntryTypes.Acc, WorkEntryTypes.Inspection] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, WorkEntryType: WorkEntryType.ComplianceEvent } &&
            ((ComplianceEvent)entry).ComplianceEventType is ComplianceEventType.AnnualComplianceCertification
            or ComplianceEventType.Inspection);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Include_ComplianceAndNonCompliance_Match()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Include = [WorkEntryTypes.Acc, WorkEntryTypes.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            !entry.IsDeleted &&
            (entry.WorkEntryType == WorkEntryType.Notification ||
             (entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
              ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.AnnualComplianceCertification)));

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch()
    {
        // Arrange
        var facilityId = WorkEntryData.GetData.First(entry => !entry.IsDeleted).FacilityId;
        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry => !entry.IsDeleted && entry.FacilityId == facilityId);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch()
    {
        // Arrange
        var facilityId = WorkEntryData.GetData.First().FacilityId[8..];
        var spec = new WorkEntrySearchDto { PartialFacilityId = facilityId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry => !entry.IsDeleted && entry.FacilityId.Contains(facilityId));

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

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
        var responsibleStaffId = WorkEntryData.GetData.First().ResponsibleStaff!.Id;
        var spec = new WorkEntrySearchDto { ResponsibleStaff = responsibleStaffId };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == responsibleStaffId);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

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
    public void Office_Single_Match()
    {
        // Arrange
        var officeId = WorkEntryData.GetData
            .First(entry => entry.ResponsibleStaff is { Office: not null }).ResponsibleStaff!.Office!.Id;
        var spec = new WorkEntrySearchDto { Offices = [officeId] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, ResponsibleStaff.Office: not null } &&
            entry.ResponsibleStaff.Office.Id == officeId);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_Single_NoMatch()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Offices = [Guid.Empty] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Multiple_Match()
    {
        // Arrange
        var officeId1 = WorkEntryData.GetData
            .First(entry => entry is { IsDeleted: false, ResponsibleStaff.Office: not null }).ResponsibleStaff!.Office!
            .Id;
        var officeId2 = WorkEntryData.GetData
            .First(entry => entry is { IsDeleted: false, ResponsibleStaff.Office: not null } &&
                            entry.ResponsibleStaff.Office.Id != officeId1)
            .ResponsibleStaff!.Office!.Id;

        var spec = new WorkEntrySearchDto { Offices = [officeId1, officeId2] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is { IsDeleted: false, ResponsibleStaff.Office: not null } &&
            (entry.ResponsibleStaff.Office.Id == officeId1 || entry.ResponsibleStaff.Office.Id == officeId2));

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_Multiple_NoMatch()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Offices = [Guid.Empty, SampleText.UnassignedGuid] };
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
        var eventDate = (WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;
        var spec = new WorkEntrySearchDto { EventDateFrom = eventDate, Include = [WorkEntryTypes.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
            notification.ReceivedDate >= eventDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EventDate_End()
    {
        // Arrange
        var eventDate = (WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;
        var spec = new WorkEntrySearchDto { EventDateTo = eventDate, Include = [WorkEntryTypes.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
            notification.ReceivedDate <= eventDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EventDate_StartAndEnd()
    {
        // Arrange
        var eventDate = (WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, WorkEntryType: WorkEntryType.Notification }) as Notification)!.ReceivedDate;
        var spec = new WorkEntrySearchDto
            { EventDateTo = eventDate, EventDateFrom = eventDate, Include = [WorkEntryTypes.Notification] };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            entry is Notification { IsDeleted: false } notification &&
            notification.ReceivedDate == eventDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_Start()
    {
        // Arrange
        var closedDate = WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;
        var spec = new WorkEntrySearchDto { ClosedDateFrom = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate >= closedDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_End()
    {
        // Arrange
        var closedDate = WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;
        var spec = new WorkEntrySearchDto { ClosedDateTo = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate <= closedDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedDate_StartAndEnd()
    {
        // Arrange
        var closedDate = WorkEntryData.GetData.First(entry => entry is
            { IsDeleted: false, ClosedDate: not null }).ClosedDate;
        var spec = new WorkEntrySearchDto
            { ClosedDateFrom = closedDate, ClosedDateTo = closedDate };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry =>
            !entry.IsDeleted && entry.ClosedDate == closedDate);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_Match()
    {
        // Arrange
        var note = WorkEntryData.GetData.First(entry => !string.IsNullOrWhiteSpace(entry.Notes)).Notes[..3];
        var spec = new WorkEntrySearchDto { Notes = note };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData.Where(entry => entry.Notes.Contains(note));

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

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
