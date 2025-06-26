using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsData;

namespace AppServicesTests.CommonFilters;

public class DeletedStatusFilterTests
{
    private static Func<SearchEntity, bool> GetExpression(DeleteStatus? spec) =>
        PredicateBuilder.True<SearchEntity>().ByDeletedStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsNotDeleted()
    {
        // Arrange
        var expected = SearchData.Where(entity => !entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetExpression(null));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void All_ReturnsAll()
    {
        // Arrange
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(DeleteStatus.All));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Deleted_ReturnsDeleted()
    {
        // Arrange
        var expected = SearchData.Where(entity => entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetExpression(DeleteStatus.Deleted));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
