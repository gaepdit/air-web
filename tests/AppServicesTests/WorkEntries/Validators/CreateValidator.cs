using AirWeb.AppServices.DomainEntities.WorkEntries.PermitRevocations;
using AirWeb.AppServices.DomainEntities.WorkEntries.Validators;
using AirWeb.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto { Notes = TextData.Paragraph };

        var validator = new WorkEntryCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.ShouldNotHaveValidationErrorFor(dto => dto.Notes);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto { Notes = string.Empty };

        var validator = new WorkEntryCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Notes);
    }
}
