using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.TestData.Compliance;

namespace AppServicesTests.Fces.Search;

public class FceFilterTests
{
    private readonly FceSearchDto _baseSpec = new() { DeleteStatus = DeleteStatus.All };

    [Test]
    public void Year_Match()
    {
        // Arrange
        var year = FceData.GetData.First().Year;
        var spec = _baseSpec with { Year = year };
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
        var spec = _baseSpec with { Year = -1 };
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
        var spec = _baseSpec with { ReviewedBy = reviewedById };
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
        var spec = _baseSpec with { ReviewedBy = Guid.Empty.ToString() };
        var expression = FceFilters.SearchPredicate(spec);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Office_Match()
    {
        // Arrange
        var officeId = FceData.GetData
            .First(fce => fce.ReviewedBy is { Office: not null }).ReviewedBy!.Office!.Id;
        var spec = _baseSpec with { Office = officeId };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce =>
            fce.ReviewedBy is { Office: not null } && fce.ReviewedBy.Office.Id == officeId);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Office_NoMatch()
    {
        // Arrange
        var spec = _baseSpec with { Office = Guid.Empty };
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
        var completedDate = FceData.GetData.First().CompletedDate;
        var spec = _baseSpec with { DateFrom = completedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => fce.CompletedDate >= completedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void CompletedDate_End()
    {
        // Arrange
        var completedDate = FceData.GetData.First().CompletedDate;
        var spec = _baseSpec with { DateTo = completedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => fce.CompletedDate <= completedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void CompletedDate_StartAndEnd()
    {
        // Arrange
        var closedDate = FceData.GetData.First().CompletedDate;
        var spec = _baseSpec with { DateTo = closedDate, DateFrom = closedDate };
        var expression = FceFilters.SearchPredicate(spec);

        var expected = FceData.GetData.Where(fce => fce.CompletedDate == closedDate);

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_Yes()
    {
        // Arrange
        var spec = _baseSpec with { Onsite = YesNoAny.Yes };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce => fce is { OnsiteInspection: true });

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_No()
    {
        // Arrange
        var spec = _baseSpec with { Onsite = YesNoAny.No };
        var expression = FceFilters.SearchPredicate(spec);
        var expected = FceData.GetData.Where(fce => fce is { OnsiteInspection: false });

        // Act
        var result = FceData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
