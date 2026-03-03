using AirWeb.AppServices.Core.Search;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.Core.CommonFilters.TestData;

namespace AppServicesTests.Core.CommonFilters;

public class ClosedStatusFilterTests
{
    private static Func<SearchEntity, bool> GetPredicate(ClosedOpenAny? spec) =>
        PredicateBuilder.True<SearchEntity>().ByClosedStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_ReturnsClosed()
    {
        // Arrange
        var expected = SearchData.Where(entity => entity.IsClosed);

        // Act
        var result = SearchData.Where(GetPredicate(ClosedOpenAny.Closed)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Open_ReturnsOpen()
    {
        // Arrange
        var expected = SearchData.Where(entity => !entity.IsClosed);

        // Act
        var result = SearchData.Where(GetPredicate(ClosedOpenAny.Open)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }
}
