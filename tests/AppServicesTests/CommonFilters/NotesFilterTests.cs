using AirWeb.AppServices.CommonSearch;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesTests.CommonFilters.CommonFilterTestsHelper;

namespace AppServicesTests.CommonFilters;

public class NotesFilterTests
{
    private record SearchDto
    {
        public string? Notes { get; init; }
    }

    private static Func<SearchEntity, bool> GetExpression(SearchDto spec) =>
        PredicateBuilder.True<SearchEntity>().ByNotesText(spec.Notes).Compile();

    [Test]
    public void Notes_NullSearch_ReturnsAll()
    {
        // Arrange
        const string? searchTerm = null;
        var spec = new SearchDto { Notes = searchTerm };
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_EmptySearch_ReturnsAll()
    {
        // Arrange
        const string? searchTerm = "";
        var spec = new SearchDto { Notes = searchTerm };
        var expected = SearchData;

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_Match()
    {
        // Arrange
        const string? searchTerm = "abc";
        var spec = new SearchDto { Notes = searchTerm };
        var expected = SearchData.Where(entity => entity.Notes != null && entity.Notes.Contains(searchTerm));

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Notes_NoMatch()
    {
        // Arrange
        const string? searchTerm = "999";
        var spec = new SearchDto { Notes = searchTerm };

        // Act
        var result = SearchData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
