using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceMonitoring.Search;

public class ResponsibleStaffFilterTests
{
    private static Func<ComplianceWork, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<ComplianceWork>().ByStaff(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

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
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = ComplianceWorkData.GetData.First(e => e.ResponsibleStaff != null).ResponsibleStaff!.Id;

        var expected = ComplianceWorkData.GetData.Where(e =>
            e.ResponsibleStaff != null && e.ResponsibleStaff.Id == spec);

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
        var result = ComplianceWorkData.GetData.Where(GetPredicate(Guid.Empty.ToString()));

        // Assert
        result.Should().BeEmpty();
    }
}
