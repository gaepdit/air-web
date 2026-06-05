using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Compliance.Fces.Validators;

public class FceSearchValidatorTests
{
    private FceSearchValidator _sut;
    private FceSearchValidator _sutFalse;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        var service = Substitute.For<IFacilityService>();
        service.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(true);
        _sut = new FceSearchValidator(service);

        var serviceFalse = Substitute.For<IFacilityService>();
        serviceFalse.ExistsAsync(Arg.Any<FacilityId>())
            .Returns(false);
        _sutFalse = new FceSearchValidator(serviceFalse);
    }

    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new FceSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
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
        var model = new FceSearchDto
        {
            FacilityId = SampleText.ValidFacilityId,
            DateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            DateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var results = await _sutFalse.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.DateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.DateTo);
        results.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }

    [Test]
    public async Task GivenInvalidFacilityId_DoesNotThrowException()
    {
        // Arrange
        var model = new FceSearchDto
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
        var model = new FceSearchDto
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
