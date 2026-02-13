using AirWeb.AppServices.FacilitySearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.FacilityFilter.FacilityFilterTestsData;

namespace AppServicesTests.FacilityFilter;

public class FacilityIdFilterTests
{
    private static Func<SearchEntity, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<SearchEntity>().ByFacilityId(spec).Compile();

    [Test]
    public void NullSearch_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmptySearch_ReturnsAll()
    {
        // Arrange
        const string? spec = "";
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FullMatch_WithHyphen()
    {
        // Arrange
        const string? spec = "001-00001";
        var expected = SearchData.Where(entity => entity.FacilityId == spec);

        // Act
        var result = SearchData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FullMatch_WithoutHyphen()
    {
        // Arrange
        const string? spec = "00100001";
        var expected = SearchData.Where(entity => entity.FacilityId.Replace("-", "") == spec);

        // Act
        var result = SearchData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void SimplifiedMatch()
    {
        // Arrange
        const string? spec = "1-1";
        var expected = SearchData.Where(entity => entity.FacilityId.Equals("001-00001"));

        // Act
        var result = SearchData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Arrange
        const string? spec = "999-99999";

        // Act
        var result = SearchData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void IncompleteFacilityId()
    {
        // Arrange
        const string? spec = "999";

        // Act
        var result = SearchData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
