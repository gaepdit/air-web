using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsHelper;

namespace AppServicesTests.CommonFilters;

public class DeletedStatusFilterTests
{
    private record SearchDto : IDeleteStatus
    {
        public DeleteStatus? DeleteStatus { get; set; }
    }

    private static Func<SearchEntity, bool> GetExpression(SearchDto spec) =>
        PredicateBuilder.True<SearchEntity>().ByDeletedStatus(spec.DeleteStatus).Compile();

    [Test]
    public void DefaultSpec_ReturnsNotDeleted()
    {
        // Arrange
        var spec = new SearchDto();
        var expected = SearchData.Where(entity => !entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_All_ReturnsAll()
    {
        // Arrange
        var spec = new SearchDto { DeleteStatus = DeleteStatus.All };
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeleteStatus_Deleted()
    {
        // Arrange
        var spec = new SearchDto { DeleteStatus = DeleteStatus.Deleted };
        var expected = SearchData.Where(entity => entity.IsDeleted);

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
