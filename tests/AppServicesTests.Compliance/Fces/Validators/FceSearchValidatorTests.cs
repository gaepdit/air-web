using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;
using Microsoft.Extensions.Caching.Memory;

namespace AppServicesTests.Compliance.Fces.Validators;

public class FceSearchValidatorTests
{
    private IFacilityService _service = null!;
    private FceSearchValidator _sut;
    private IFacilityService _serviceFalse = null!;
    private FceSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _service = Substitute.For<IFacilityService>();
        _service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new FceSearchValidator(_service);

        _serviceFalse = Substitute.For<IFacilityService>();
        _serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new FceSearchValidator(_serviceFalse);
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new FceSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            DateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            DateTo = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DateFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new FceSearchDto
        {
            DateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DateFrom);
    }

    [Test]
    public async Task DateToInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new FceSearchDto
        {
            DateTo= DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DateTo);
    }

    [Test]
    public async Task DateToIsBeforeDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new FceSearchDto
        {
            DateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            DateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.DateTo);
    }

    [Test]
    public async Task FacilityIdDoesNotExist_ReturnsAsInvalid()
    {
        // Arrange
        var model = new FceSearchDto
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
