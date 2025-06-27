using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsData;

namespace AppServicesTests.CommonFilters;

public class DeletedStatusFilterTests
{
    private static Func<SearchEntity, bool> GetPredicate(DeleteStatus? spec) =>
        PredicateBuilder.True<SearchEntity>().ByDeletedStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsNotDeleted()
    {
        // Arrange
        var expected = SearchData.Where(entity => !entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void All_ReturnsAll()
    {
        // Arrange
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetPredicate(DeleteStatus.All)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Deleted_ReturnsDeleted()
    {
        // Arrange
        var expected = SearchData.Where(entity => entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetPredicate(DeleteStatus.Deleted)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }
}
