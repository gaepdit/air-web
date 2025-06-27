using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Fces.Search;

public class YearFilterTests
{
    private static Func<Fce, bool> GetPredicate(int? spec) =>
        PredicateBuilder.True<Fce>().ByYear(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Act
        var result = FceData.GetData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(FceData.GetData);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = FceData.GetData.First().Year;
        var expected = FceData.GetData.Where(fce => fce.Year == spec);

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
        var result = FceData.GetData.Where(GetPredicate(0));

        // Assert
        result.Should().BeEmpty();
    }
}
