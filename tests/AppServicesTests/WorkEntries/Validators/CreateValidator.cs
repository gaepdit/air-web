using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using AirWeb.AppServices.WorkEntries.Validators;
using AirWeb.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new BaseWorkEntryCreateDto
        {
            Notes = TextData.Paragraph,
        };

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
        var model = new BaseWorkEntryCreateDto
        {
            Notes = string.Empty,
        };

        var validator = new WorkEntryCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Notes);
    }
}
