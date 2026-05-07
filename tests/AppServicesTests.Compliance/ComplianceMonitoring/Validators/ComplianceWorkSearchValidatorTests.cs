using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkSearchValidatorTests
{
    private readonly IFacilityService _service;

    private static readonly ComplianceWorkValidator _validator = new(_service);

    [Test]
    public async Task EventDateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            EventDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EventDateFrom);
    }
}
