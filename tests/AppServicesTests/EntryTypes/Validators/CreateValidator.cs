using FluentValidation.TestHelper;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.EntryTypes.Validators;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.TestData.Constants;

namespace AppServicesTests.EntryTypes.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((EntryType?)null);
        var model = new EntryTypeCreateDto(TextData.ValidName);

        var validator = new EntryTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new EntryType(Guid.Empty, TextData.ValidName));
        var model = new EntryTypeCreateDto(TextData.ValidName);

        var validator = new EntryTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((EntryType?)null);
        var model = new EntryTypeCreateDto(TextData.ShortName);

        var validator = new EntryTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
