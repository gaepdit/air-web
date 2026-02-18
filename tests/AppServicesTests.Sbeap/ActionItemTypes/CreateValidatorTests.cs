using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using FluentValidation.TestHelper;

namespace AppServicesSbeapTests.ActionItemTypes;

public class CreateValidatorTests
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeCreateDto { Name = TestData.ValidName };

        // Act
        var validator = new ActionItemTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        //Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.Empty, TestData.ValidName));
        var model = new ActionItemTypeCreateDto { Name = TestData.ValidName };

        // Act
        var validator = new ActionItemTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        //Assert
        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeCreateDto { Name = TestData.ShortName };

        var validator = new ActionItemTypeCreateValidator(repoMock);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
