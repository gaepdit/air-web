using FluentValidation.TestHelper;
using IaipDataService.Permits;

namespace IaipDataServiceTests.Permits;

public class PermitSearchValidatorTests
{
    private PermitSearchValidator _sut;
    private const string SampleText = nameof(SampleText);

    [SetUp]
    public void SetUp() => _sut = new PermitSearchValidator();

    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitSearchDto
        {
            Name = SampleText,
            Permit = SampleText,
            DateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
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
        var model = new PermitSearchDto
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
    public async Task DateToBeforeDateFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitSearchDto
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
    public async Task MultipleErrors_ReturnsValidationErrorsForAllFields()
    {
        // Arrange
        var model = new PermitSearchDto
        {
            DateFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            DateTo = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.DateFrom);
        results.ShouldHaveValidationErrorFor(dto => dto.DateTo);
    }
}
