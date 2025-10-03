using AirWeb.AppServices.Compliance.ComplianceWork.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceWork.Search;

public class ResponsibleStaffFilterTests
{
    private static Func<WorkEntry, bool> GetPredicate(string? spec) =>
        PredicateBuilder.True<WorkEntry>().ByStaff(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

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
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = WorkEntryData.GetData.First(e => e.ResponsibleStaff != null).ResponsibleStaff!.Id;

        var expected = WorkEntryData.GetData.Where(e =>
            e.ResponsibleStaff != null && e.ResponsibleStaff.Id == spec);

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(Guid.Empty.ToString()));

        // Assert
        result.Should().BeEmpty();
    }
}
