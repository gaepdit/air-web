using FluentValidation;
using FluentValidation.TestHelper;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.EntryTypes.Validators;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.TestData.Constants;

namespace AppServicesTests.EntryTypes.Validators;

public class UpdateValidator
{
    private static ValidationContext<EntryTypeUpdateDto> GetContext(EntryTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((EntryType?)null);
        var model = new EntryTypeUpdateDto(TextData.ValidName, true);

        var result = await new EntryTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new EntryType(Guid.NewGuid(), TextData.ValidName));
        var model = new EntryTypeUpdateDto(TextData.ValidName, true);

        var result = await new EntryTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new EntryType(Guid.Empty, TextData.ValidName));
        var model = new EntryTypeUpdateDto(TextData.ValidName, true);

        var result = await new EntryTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((EntryType?)null);
        var model = new EntryTypeUpdateDto(TextData.ShortName, true);

        var result = await new EntryTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
