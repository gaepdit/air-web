using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsHelper;

namespace AppServicesTests.CommonFilters;

public class ClosedStatusFilterTests
{
    private record SearchDto : IClosedStatus
    {
        public ClosedOpenAny? Closed { get; init; }
    }

    private static Func<SearchEntity, bool> GetExpression(SearchDto spec) =>
        PredicateBuilder.True<SearchEntity>().ByClosedStatus(spec.Closed).Compile();

    [Test]
    public void DefaultSpec_ReturnsAll()
    {
        // Arrange
        var spec = new SearchDto();
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OpenStatus_Closed_ReturnsClosed()
    {
        // Arrange
        var spec = new SearchDto { Closed = ClosedOpenAny.Closed };
        var expected = SearchData.Where(entity => entity.IsClosed);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OpenStatus_Open_ReturnsOpen()
    {
        // Arrange
        var spec = new SearchDto { Closed = ClosedOpenAny.Open };
        var expected = SearchData.Where(entity => !entity.IsClosed);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
