using AirWeb.AppServices.Compliance.Search;
using AirWeb.TestData.Entities;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.ComplianceSearch;

public class FceFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new FceSearchDto();
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce => !fce.IsDeleted);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_All_ReturnsAll()
    {
        // Arrange
        var spec = new FceSearchDto { DeleteStatus = DeleteStatus.All };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData;

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_Deleted()
    {
        // Arrange
        var spec = new FceSearchDto { DeleteStatus = DeleteStatus.Deleted };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce => fce.IsDeleted);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch()
    {
        // Arrange
        var facilityId = FceData.GetData.First(fce => !fce.IsDeleted).FacilityId;
        var spec = new FceSearchDto { PartialFacilityId = facilityId };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => !fce.IsDeleted && fce.FacilityId == facilityId);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch()
    {
        // Arrange
        var facilityId = FceData.GetData.First(fce => !fce.IsDeleted).FacilityId[8..];
        var spec = new FceSearchDto { PartialFacilityId = facilityId };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => !fce.IsDeleted && fce.FacilityId.Contains(facilityId));

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { PartialFacilityId = "99999" };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Year_Match()
    {
        // Arrange
        var year = FceData.GetData.First().Year;
        var spec = new FceSearchDto { Year = year };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => fce.Year == year);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Year_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { Year = -1 };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void ReviewedBy_Match()
    {
        // Arrange
        var reviewedById = FceData.GetData.First().ReviewedBy!.Id;
        var spec = new FceSearchDto { ReviewedBy = reviewedById };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            fce.ReviewedBy != null && fce.ReviewedBy.Id == reviewedById);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ReviewedBy_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { ReviewedBy = Guid.Empty.ToString() };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Single_Match()
    {
        // Arrange
        var officeId = FceData.GetData
            .First(fce => fce.ReviewedBy is { Office: not null }).ReviewedBy!.Office!.Id;
        var spec = new FceSearchDto { Offices = [officeId] };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            fce.ReviewedBy is { Office: not null } && fce.ReviewedBy.Office.Id == officeId);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_Single_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { Offices = [Guid.Empty] };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Multiple_Match()
    {
        // Arrange
        var officeId1 = FceData.GetData
            .First(fce => fce.ReviewedBy is { Office: not null }).ReviewedBy!.Office!.Id;
        var officeId2 = FceData.GetData
            .First(fce => fce.ReviewedBy is { Office: not null } && fce.ReviewedBy.Office.Id != officeId1)
            .ReviewedBy!.Office!.Id;

        var spec = new FceSearchDto { Offices = [officeId1, officeId2] };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            fce.ReviewedBy is { Office: not null } &&
            (fce.ReviewedBy.Office.Id == officeId1 || fce.ReviewedBy.Office.Id == officeId2));

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_Multiple_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { Offices = [Guid.Empty, SampleText.UnassignedGuid] };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void CompletedDate_Start()
    {
        // Arrange
        var completedDate = FceData.GetData.First(fce => !fce.IsDeleted).CompletedDate;
        var spec = new FceSearchDto { DateFrom = completedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            !fce.IsDeleted && fce.CompletedDate >= completedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void CompletedDate_End()
    {
        // Arrange
        var completedDate = FceData.GetData.First(fce => !fce.IsDeleted).CompletedDate;
        var spec = new FceSearchDto { DateTo = completedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            !fce.IsDeleted && fce.CompletedDate <= completedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void CompletedDate_StartAndEnd()
    {
        // Arrange
        var closedDate = FceData.GetData.First(fce => !fce.IsDeleted).CompletedDate;
        var spec = new FceSearchDto { DateTo = closedDate, DateFrom = closedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            !fce.IsDeleted && fce.CompletedDate == closedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_Yes()
    {
        // Arrange
        var spec = new FceSearchDto { Onsite = YesNoAny.Yes };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce =>
            fce is { IsDeleted: false, OnsiteInspection: true });

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_No()
    {
        // Arrange
        var spec = new FceSearchDto { Onsite = YesNoAny.No };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce =>
            fce is { IsDeleted: false, OnsiteInspection: false });

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_Match()
    {
        // Arrange
        var note = FceData.GetData.First(fce => !string.IsNullOrWhiteSpace(fce.Notes)).Notes[..3];
        var spec = new FceSearchDto { Notes = note };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => fce.Notes.Contains(note));

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_NoMatch()
    {
        // Arrange
        var spec = new FceSearchDto { Notes = Guid.NewGuid().ToString() };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }
}
