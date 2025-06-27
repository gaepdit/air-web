using AirWeb.AppServices.Enforcement.Search;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Enforcement.Search;

public class DiscoveryDateFromFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<CaseFile>().FromDiscoveryDate(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
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
        var spec = CaseFileData.GetData.First(e => e.DiscoveryDate != null).DiscoveryDate!.Value.AddDays(-1);

        var expected = CaseFileData.GetData.Where(e =>
            e.DiscoveryDate != null && e.DiscoveryDate >= spec);

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
        var spec = CaseFileData.GetData.First(e => e.DiscoveryDate != null).DiscoveryDate!.Value;

        var expected = CaseFileData.GetData.Where(e =>
            e.DiscoveryDate != null && e.DiscoveryDate >= spec);

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
        var result = CaseFileData.GetData.Where(GetPredicate(DateOnly.MaxValue));

        // Assert
        result.Should().BeEmpty();
    }
}
