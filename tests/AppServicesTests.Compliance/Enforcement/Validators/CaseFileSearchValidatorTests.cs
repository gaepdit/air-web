using System;
using System.Collections.Generic;
using System.Text;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class CaseFileSearchValidatorTests
{
    private IFacilityService _service = null!;
    private CaseFileSearchValidator _sut;
    private IFacilityService _serviceFalse = null!;
    private CaseFileSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _service = Substitute.For<IFacilityService>();
        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new CaseFileSearchValidator(_service);

        _serviceFalse = Substitute.For<IFacilityService>();
        _serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new CaseFileSearchValidator(_serviceFalse);
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            DiscoveryDateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            DiscoveryDateTo = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DiscoveryDateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            DiscoveryDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DiscoveryDateFrom);
    }

    [Test]
    public async Task DiscoveryDateToInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            DiscoveryDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DiscoveryDateTo);
    }

    [Test]
    public async Task DiscoveryDateToIsBeforeDiscoveryDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            DiscoveryDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            DiscoveryDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.DiscoveryDateTo);
    }

    [Test]
    public async Task EnforcementDateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            EnforcementDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EnforcementDateFrom);
    }

    [Test]
    public async Task EnforcementDateToInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            EnforcementDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EnforcementDateTo);
    }

    [Test]
    public async Task EnforcementDateToIsBeforeEnforcementDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            EnforcementDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            EnforcementDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.EnforcementDateTo);
    }

    [Test]
    public async Task FacilityIdDoesNotExist_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = "00999999",
        };

        // Act
        var results = await _sutFalse.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }
}
