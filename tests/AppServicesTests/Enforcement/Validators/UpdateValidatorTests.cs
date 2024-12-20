using AirWeb.AppServices.Enforcement.Command;
using FluentValidation.TestHelper;

namespace AppServicesTests.Enforcement.Validators;

public class UpdateValidatorTests
{
    private CaseFileUpdateValidator _validator;

    [SetUp]
    public void SetUp() => _validator = new CaseFileUpdateValidator();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseFileUpdateDto
        {
            ResponsibleStaffId = "1",
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DiscoveryDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileUpdateDto
        {
            ResponsibleStaffId = "1",
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task MissingStaff_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseFileUpdateDto
        {
            ResponsibleStaffId = null,
            DiscoveryDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
