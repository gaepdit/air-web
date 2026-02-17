using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceMonitoring.Search;

public class OfficeFilterTests
{
    private static Func<ComplianceWork, bool> GetPredicate(Guid? spec) =>
        PredicateBuilder.True<ComplianceWork>().ByOffice(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = ComplianceWorkData.GetData
            .First(e => e.ResponsibleStaff is { Office: not null })
            .ResponsibleStaff!.Office!.Id;

        var expected = ComplianceWorkData.GetData.Where(e =>
            e.ResponsibleStaff is { Office: not null } && e.ResponsibleStaff.Office.Id == spec);

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(Guid.Empty));

        // Assert
        result.Should().BeEmpty();
    }
}
