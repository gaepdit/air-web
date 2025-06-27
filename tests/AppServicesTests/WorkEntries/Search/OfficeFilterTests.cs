using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.WorkEntries.Search;

public class OfficeFilterTests
{
    private static Func<WorkEntry, bool> GetPredicate(Guid? spec) =>
        PredicateBuilder.True<WorkEntry>().ByOffice(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = WorkEntryData.GetData
            .First(e => e.ResponsibleStaff is { Office: not null })
            .ResponsibleStaff!.Office!.Id;

        var expected = WorkEntryData.GetData.Where(e =>
            e.ResponsibleStaff is { Office: not null } && e.ResponsibleStaff.Office.Id == spec);

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
        var result = WorkEntryData.GetData.Where(GetPredicate(Guid.Empty));

        // Assert
        result.Should().BeEmpty();
    }
}
