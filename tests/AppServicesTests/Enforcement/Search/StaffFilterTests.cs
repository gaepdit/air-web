using AirWeb.AppServices.Enforcement.Search;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Enforcement.Search;

public class StaffFilterTests
{
    private static Func<CaseFile, bool> GetExpression(string? spec) =>
        PredicateBuilder.True<CaseFile>().ByStaff(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        const string? spec = null;
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmptySpec_ReturnsAll()
    {
        // Arrange
        const string? spec = "";
        var expected = CaseFileData.GetData;

        // Act
        var result = CaseFileData.GetData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Match()
    {
        // Arrange
        var spec = CaseFileData.GetData.First(e => e.ResponsibleStaff != null).ResponsibleStaff!.Id;

        var expected = CaseFileData.GetData.Where(e =>
            e.ResponsibleStaff != null && e.ResponsibleStaff.Id == spec);

        // Act
        var result = CaseFileData.GetData.Where(GetExpression(spec));

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Act
        var result = CaseFileData.GetData.Where(GetExpression(Guid.Empty.ToString()));

        // Assert
        result.Should().BeEmpty();
    }
}
