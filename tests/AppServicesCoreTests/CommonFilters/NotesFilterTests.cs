using AirWeb.AppServices.Core.Search;
using GaEpd.AppLibrary.Domain.Predicates;
using static AppServicesCoreTests.CommonFilters.FilterTestsData;

namespace AppServicesCoreTests.CommonFilters;

public class NotesFilterTests
{
    private static Func<SearchEntity, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<SearchEntity>().ByNotesText(spec).Compile();

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
    public void Match()
    {
        // Arrange
        const string? spec = "abc";
        var expected = SearchData.Where(entity => entity.Notes != null && entity.Notes.Contains(spec));

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
        const string? spec = "xxx";

        // Act
        var result = SearchData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
