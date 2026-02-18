using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesSbeapTests.ActionItemTypes;

internal class UpdateValidatorTests
{
    private static ValidationContext<ActionItemTypeUpdateDto> GetContext(ActionItemTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto { Name = TestData.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.NewGuid(), TestData.ValidName));
        var model = new ActionItemTypeUpdateDto { Name = TestData.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.Empty, TestData.ValidName));
        var model = new ActionItemTypeUpdateDto { Name = TestData.ValidName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto { Name = TestData.ShortName, Active = true };

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
