using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Core.Search;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Fces.Search;

public class OnsiteStatusFilterTests
{
    private static Func<Fce, bool> GetPredicate(YesNoAny? spec) =>
        PredicateBuilder.True<Fce>().ByOnsiteStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = FceData.GetData;

        // Act
        var result = FceData.GetData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_Yes()
    {
        // Arrange
        var expected = FceData.GetData.Where(fce => fce is { OnsiteInspection: true });

        // Act
        var result = FceData.GetData.Where(GetPredicate(YesNoAny.Yes));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Onsite_No()
    {
        // Arrange
        var expected = FceData.GetData.Where(fce => fce is { OnsiteInspection: false });

        // Act
        var result = FceData.GetData.Where(GetPredicate(YesNoAny.No));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
