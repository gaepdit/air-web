using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AppServicesTests.Sbeap.TestData;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.Agencies;

internal class UpdateValidatorTests
{
    private static ValidationContext<AgencyUpdateDto> GetContext(AgencyUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var model = new AgencyUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Agency(Guid.NewGuid(), Constants.ValidName));
        var model = new AgencyUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Agency(Guid.Empty, Constants.ValidName));
        var model = new AgencyUpdateDto { Name = Constants.ValidName, Active = true };

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var model = new AgencyUpdateDto { Name = Constants.ShortName, Active = true };

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
