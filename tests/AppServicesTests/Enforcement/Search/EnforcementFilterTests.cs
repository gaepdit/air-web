using AirWeb.AppServices.Enforcement.Search;
using AirWeb.TestData.Enforcement;

namespace AppServicesTests.Enforcement.Search;

public class EnforcementFilterTests
{
    [Test]
    public void FacilityId_FullMatch_WithHyphen()
    {
        // Arrange
        var facilityId = CaseFileData.GetData.First().FacilityId;
        var spec = new EnforcementSearchDto { PartialFacilityId = facilityId };
        var expression = EnforcementFilters.SearchPredicate(spec);

        var expected = CaseFileData.GetData.Where(fce => fce.FacilityId == facilityId);

        // Act
        var result = CaseFileData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithHyphen()
    {
        // Arrange
        var facilityId = CaseFileData.GetData.First().FacilityId[..5];
        var spec = new EnforcementSearchDto { PartialFacilityId = facilityId };
        var expression = EnforcementFilters.SearchPredicate(spec);

        var expected = CaseFileData.GetData.Where(fce => fce.FacilityId.Contains(facilityId));

        // Act
        var result = CaseFileData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_FullMatch_WithoutHyphen()
    {
        // Arrange
        var facilityId = CaseFileData.GetData.First().FacilityId;
        var spec = new EnforcementSearchDto { PartialFacilityId = facilityId.Trim('-') };
        var expression = EnforcementFilters.SearchPredicate(spec);

        var expected = CaseFileData.GetData.Where(fce => fce.FacilityId == facilityId);

        // Act
        var result = CaseFileData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_PartialMatch_WithoutHyphen()
    {
        // Arrange
        var facilityId = CaseFileData.GetData.First().FacilityId[..5];
        var spec = new EnforcementSearchDto { PartialFacilityId = facilityId.Trim('-') };
        var expression = EnforcementFilters.SearchPredicate(spec);

        var expected = CaseFileData.GetData.Where(fce => fce.FacilityId.Contains(facilityId));

        // Act
        var result = CaseFileData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FacilityId_NoMatch()
    {
        // Arrange
        var spec = new EnforcementSearchDto { PartialFacilityId = "99999" };
        var expression = EnforcementFilters.SearchPredicate(spec);

        // Act
        var result = CaseFileData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEmpty();
    }
}
