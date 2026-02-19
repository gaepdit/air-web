using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Fces.Search;

public class ReviewedByFilterTests
{
    private static Func<Fce, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<Fce>().ByStaff(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = FceData.GetData;

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmptySpec_ReturnsAll()
    {
        // Arrange
        const string? spec = "";
        var expected = FceData.GetData;

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = FceData.GetData.First(e => e.ReviewedBy != null).ReviewedBy!.Id;

        var expected = FceData.GetData.Where(e =>
            e.ReviewedBy != null && e.ReviewedBy.Id == spec);

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Act
        var result = FceData.GetData.Where(GetPredicate(Guid.Empty.ToString()));

        // Assert
        result.Should().BeEmpty();
    }
}
