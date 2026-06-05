using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class CaseFileSearchValidatorTests
{
    private CaseFileSearchValidator _sut;
    private CaseFileSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        var service = Substitute.For<IFacilityService>();
        service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new CaseFileSearchValidator(service);

        var serviceFalse = Substitute.For<IFacilityService>();
        serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new CaseFileSearchValidator(serviceFalse);
    }

    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseFileSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
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
            EnforcementDateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            EnforcementDateTo = DateOnly.FromDateTime(DateTime.Today),
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
            FacilityId = SampleText.ValidFacilityId,
        };

        // Act
        var results = await _sutFalse.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }

    [Test]
    public async Task MultipleErrors_ReturnsValidationErrorsForAllFields()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            DiscoveryDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            DiscoveryDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            EnforcementDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            EnforcementDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var results = await _sutFalse.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.DiscoveryDateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.DiscoveryDateTo);
        results.ShouldHaveValidationErrorFor(dto => dto.EnforcementDateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.EnforcementDateTo);
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }

    [Test]
    public async Task GivenInvalidFacilityId_DoesNotThrowException()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = SampleText.InvalidFacilityId,
        };

        // Act
        var func = async () => await _sut.TestValidateAsync(model);

        // Assert
        await func.Should().NotThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task FacilityId_Invalid_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileSearchDto
        {
            FacilityId = SampleText.InvalidFacilityId,
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }
}
