using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsHelper;

namespace AppServicesTests.CommonFilters;

public class FacilityIdFilterTests
{
    private record SearchDto
    {
        public string? PartialFacilityId { get; init; }
    }

    private static Func<SearchEntity, bool> GetExpression(SearchDto spec) =>
        PredicateBuilder.True<SearchEntity>().ByFacilityId(spec.PartialFacilityId).Compile();

    [Test]
    public void FacilityId_NullSearch_ReturnsAll()
    {
        // Arrange
        const string? searchTerm = null;
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_EmptySearch_ReturnsAll()
    {
        // Arrange
        const string? searchTerm = "";
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch_WithHyphen()
    {
        // Arrange
        const string? searchTerm = "001-00001";
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData.Where(entity => entity.FacilityId == searchTerm);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithHyphen()
    {
        // Arrange
        const string? searchTerm = "001-0";
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData.Where(entity => entity.FacilityId.Contains(searchTerm));

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch_WithoutHyphen()
    {
        // Arrange
        const string? searchTerm = "00100001";
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData.Where(entity => entity.FacilityId.Replace("-", "") == searchTerm);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithoutHyphen()
    {
        // Arrange
        const string? searchTerm = "003";
        var spec = new SearchDto { PartialFacilityId = searchTerm };
        var expected = SearchData.Where(entity => entity.FacilityId.Contains(searchTerm));

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_NoMatch()
    {
        // Arrange
        const string? searchTerm = "999";
        var spec = new SearchDto { PartialFacilityId = searchTerm };

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
