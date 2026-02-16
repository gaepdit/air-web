using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Enforcement.Search;

public class CaseFileStatusFilterTests
{
    private static Func<CaseFile, bool> GetPredicate(CaseFileStatus? spec) =>
        PredicateBuilder.True<CaseFile>().ByCaseFileStatus(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;

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
    public void Closed()
    {
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;

        // Arrange
        var expected = CaseFileData.GetData.Where(entity => entity.CaseFileStatus == CaseFileStatus.Closed);

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(CaseFileStatus.Closed)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Draft()
    {
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;

        // Arrange
        var expected = CaseFileData.GetData.Where(entity => entity.CaseFileStatus == CaseFileStatus.Draft);

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(CaseFileStatus.Draft)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void SubjectToComplianceSchedule()
    {
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;

        // Arrange
        var expected =
            CaseFileData.GetData.Where(entity => entity.CaseFileStatus == CaseFileStatus.SubjectToComplianceSchedule);

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(CaseFileStatus.SubjectToComplianceSchedule)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Open()
    {
        // Hydrate the enforcement action data
        _ = EnforcementActionData.GetData;

        // Arrange
        var expected = CaseFileData.GetData.Where(entity => entity.CaseFileStatus == CaseFileStatus.Open);

        // Act
        var result = CaseFileData.GetData.Where(GetPredicate(CaseFileStatus.Open)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }
}
