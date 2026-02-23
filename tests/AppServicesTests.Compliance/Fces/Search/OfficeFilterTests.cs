using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Fces.Search;

public class OfficeFilterTests
{
    private static Func<Fce, bool> GetPredicate(Guid? spec) =>
        PredicateBuilder.True<Fce>().ByOffice(spec).Compile();

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
    public void Match()
    {
        // Arrange
        var spec = FceData.GetData
            .First(e => e.ReviewedBy is { Office: not null })
            .ReviewedBy!.Office!.Id;

        var expected = FceData.GetData.Where(e =>
            e.ReviewedBy is { Office: not null } && e.ReviewedBy.Office.Id == spec);

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
        var result = FceData.GetData.Where(GetPredicate(Guid.Empty));

        // Assert
        result.Should().BeEmpty();
    }
}
