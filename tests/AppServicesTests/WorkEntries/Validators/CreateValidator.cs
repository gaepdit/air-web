using FluentValidation.TestHelper;
using MyApp.AppServices.WorkEntries.CommandDto;
using MyApp.AppServices.WorkEntries.Validators;
using MyApp.TestData.Constants;

namespace AppServicesTests.WorkEntries.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new WorkEntryCreateDto
        {
            EntryTypeId = Guid.NewGuid(),
            Notes = TextData.Paragraph,
        };

        var validator = new WorkEntryCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.ShouldNotHaveValidationErrorFor(dto => dto.EntryTypeId);
        result.ShouldNotHaveValidationErrorFor(dto => dto.Notes);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        // Arrange
        var model = new WorkEntryCreateDto
        {
            EntryTypeId = Guid.NewGuid(),
            Notes = string.Empty,
        };

        var validator = new WorkEntryCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Notes);
    }
}
