using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsData;

namespace AppServicesTests.CommonFilters;

public class ClosedStatusFilterTests
{
    private static Func<SearchEntity, bool> GetExpression(ClosedOpenAny? spec) =>
        PredicateBuilder.True<SearchEntity>().ByClosedStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(null));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Closed_ReturnsClosed()
    {
        // Arrange
        var expected = SearchData.Where(entity => entity.IsClosed);

        // Act
        var result = SearchData.Where(GetExpression(ClosedOpenAny.Closed));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Open_ReturnsOpen()
    {
        // Arrange
        var expected = SearchData.Where(entity => !entity.IsClosed);

        // Act
        var result = SearchData.Where(GetExpression(ClosedOpenAny.Open));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
