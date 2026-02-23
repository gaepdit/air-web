using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Enforcement.Search;

public class MinEnforcementDateFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<CaseFile>().MinEnforcementDate(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        _ = CaseFileData.GetData; // hydrate case file data
        _ = EnforcementActionData.GetData; // hydrate enforcement action data

        DateOnly? spec = null;
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void RangeMatch()
    {
        // Arrange
        _ = CaseFileData.GetData; // hydrate case file data
        _ = EnforcementActionData.GetData; // hydrate enforcement action data

        var spec = CaseFileData.GetData.First(e => e.EnforcementDate != null).EnforcementDate!.Value.AddDays(-1);

        var expected = CaseFileData.GetData.Where(e =>
            e.EnforcementDate != null && e.EnforcementDate >= spec);

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ExactMatch()
    {
        // Arrange
        _ = CaseFileData.GetData; // hydrate case file data
        _ = EnforcementActionData.GetData; // hydrate enforcement action data

        var spec = CaseFileData.GetData.First(e => e.EnforcementDate != null).EnforcementDate!.Value;

        var expected = CaseFileData.GetData.Where(e =>
            e.EnforcementDate != null && e.EnforcementDate >= spec);

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
        // Act
        _ = CaseFileData.GetData; // hydrate case file data
        _ = EnforcementActionData.GetData; // hydrate enforcement action data

        var result = CaseFileData.GetData.Where(GetPredicate(DateOnly.MaxValue));

        // Assert
        result.Should().BeEmpty();
    }
}
