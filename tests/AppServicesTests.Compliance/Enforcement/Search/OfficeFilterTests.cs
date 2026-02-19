using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.Enforcement.Search;

public class OfficeFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(Guid? spec) =>
        PredicateBuilder.True<CaseFile>().ByOffice(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(null)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = CaseFileData.GetData
            .First(e => e.ResponsibleStaff is { Office: not null })
            .ResponsibleStaff!.Office!.Id;

        var expected = CaseFileData.GetData.Where(e =>
            e.ResponsibleStaff is { Office: not null } && e.ResponsibleStaff.Office.Id == spec);

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
        var result = CaseFileData.GetData.Where(GetPredicate(Guid.Empty));

        // Assert
        result.Should().BeEmpty();
    }
}
