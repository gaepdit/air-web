using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Validators;

public class ComplianceWorkSearchValidatorTests
{
    private ComplianceWorkSearchValidator _sut;
    private ComplianceWorkSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        var service = Substitute.For<IFacilityService>();
        service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new ComplianceWorkSearchValidator(service);

        var serviceFalse = Substitute.For<IFacilityService>();
        serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new ComplianceWorkSearchValidator(serviceFalse);
    }

    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
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
            ClosedDateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            ClosedDateTo = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

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
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.EventDateFrom);
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
        var results = await _sut.TestValidateAsync(model);

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
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ClosedDateFrom);
    }

    [Test]
    public async Task ClosedDateToIsBeforeClosedDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
        {
            ClosedDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ClosedDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.ClosedDateTo);
    }

    [Test]
    public async Task FacilityIdDoesNotExist_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
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
        var model = new ComplianceWorkSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            EventDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            EventDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ClosedDateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            ClosedDateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var results = await _sutFalse.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.EventDateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.EventDateTo);
        results.ShouldHaveValidationErrorFor(dto => dto.ClosedDateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.ClosedDateTo);
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }

    [Test]
    public async Task GivenInvalidFacilityId_DoesNotThrowException()
    {
        // Arrange
        var model = new ComplianceWorkSearchDto
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
        var model = new ComplianceWorkSearchDto
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
