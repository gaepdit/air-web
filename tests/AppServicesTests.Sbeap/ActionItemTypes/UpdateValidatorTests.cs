using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AppServicesTests.Sbeap.TestData;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.ActionItemTypes;

internal class UpdateValidatorTests
{
    private static ValidationContext<ActionItemTypeUpdateDto> GetContext(ActionItemTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.NewGuid(), Constants.ValidName));
        var model = new ActionItemTypeUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.Empty, Constants.ValidName));
        var model = new ActionItemTypeUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto { Name = Constants.ShortName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
