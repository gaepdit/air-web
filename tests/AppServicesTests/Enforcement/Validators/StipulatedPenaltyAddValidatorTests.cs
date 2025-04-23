using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using FluentValidation;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Enforcement.Validators;

public class StipulatedPenaltyAddValidatorTests
{
    private readonly ConsentOrder _consentOrder = new(Guid.Empty, new CaseFile(1, (FacilityId)"00100001", null), null)
        { ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1) };

    [Test]
    public async Task DtoWithValidValues_ReturnsAsValid()
    {
        // Arrange
        var validator = new StipulatedPenaltyAddValidator(Substitute.For<IEnforcementActionRepository>());
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today),
            Amount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ZeroPenalty_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new StipulatedPenaltyAddValidator(Substitute.For<IEnforcementActionRepository>());
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today),
            Amount = 0,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task NegativePenalty_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new StipulatedPenaltyAddValidator(Substitute.For<IEnforcementActionRepository>());
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today),
            Amount = -1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ReceivedFromFacilityDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new StipulatedPenaltyAddValidator(Substitute.For<IEnforcementActionRepository>());
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            Amount = 1,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ReceivedFromFacilityBeforeCoExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(_consentOrder);

        var validator = new StipulatedPenaltyAddValidator(repoMock);
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            Amount = 1,
        };

        ValidationContext<StipulatedPenaltyAddDto> validationContext = new(model)
            { RootContextData = { ["ConsentOrder.Id"] = Guid.Empty } };

        // Act
        var result = await validator.TestValidateAsync(validationContext);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task CoNotExecuted_ReturnsAsInvalid()
    {
        // Arrange
        ConsentOrder consentOrder = new(Guid.Empty, new CaseFile(1, (FacilityId)"00100001", null), null);

        var repoMock = Substitute.For<IEnforcementActionRepository>();
        repoMock.GetAsync(Arg.Any<Guid>(), Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(consentOrder);

        var validator = new StipulatedPenaltyAddValidator(repoMock);
        var model = new StipulatedPenaltyAddDto
        {
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            Amount = 1,
        };

        ValidationContext<StipulatedPenaltyAddDto> validationContext = new(model)
            { RootContextData = { ["ConsentOrder.Id"] = Guid.Empty } };

        // Act
        var result = await validator.TestValidateAsync(validationContext);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
