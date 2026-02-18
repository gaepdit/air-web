using AirWeb.AppServices.Core.Search;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Enforcement.Search;

public class NotesFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<CaseFile>().ByNotesText(spec).Compile();

    [Test]
    public void NullSearch_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

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
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FullMatch()
    {
        // Arrange
        // Compare against the full text of the note
        var spec = CaseFileData.GetData.First(e => e.Notes != null).Notes!;
        var expected = CaseFileData.GetData.Where(entity => entity.Notes!.Contains(spec));

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PartialMatch()
    {
        // Arrange
        // Compare against a substring within the note
        var spec = CaseFileData.GetData.First(e => e.Notes is { Length: > 3 }).Notes![1..^1];
        var expected = CaseFileData.GetData.Where(entity => entity.Notes != null && entity.Notes.Contains(spec));

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Arrange
        const string? spec = "999";

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
