using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Enforcement.Search;

public class ViolationTypeFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<CaseFile>().ByViolationType(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
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
    public void EmptySpec_ReturnsNone()
    {
        // An empty spec is the same as no match!
        // Arrange
        const string? spec = "";

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEmpty();
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = CaseFileData.GetData.First(e => e.ViolationType != null).ViolationType!.Code;
        var expected = CaseFileData.GetData
            .Where(entity => entity.ViolationType != null && entity.ViolationType.Code == spec);

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
        const string? spec = "xxx";

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
