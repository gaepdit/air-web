using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkSearchValidatorTests
{
    private readonly IFacilityService _service;
    private readonly ComplianceWorkValidator _validator;

    public ComplianceWorkSearchValidatorTests()
    {
        _service = Substitute.For<IFacilityService>();

        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);

        _validator = new ComplianceWorkValidator(_service);
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            EventDateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            EventDateTo = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    [Test]
    public async Task EventDateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            EventDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EventDateFrom);
    }
    [Test]
    public async Task EventDateToInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            EventDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EventDateTo);
    }
    [Test]
    public async Task EventDateToIsBeforeEventDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            EventDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            EventDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.EventDateTo);
    }
    [Test]
    public async Task ClosedDateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            ClosedDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ClosedDateFrom);
    }
    [Test]
    public async Task ClosedDateToInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            ClosedDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ClosedDateTo);
    }
}
