using AirWeb.AppServices.Compliance.Fces;
using FluentValidation.TestHelper;

namespace AppServicesTests.Fces.Validators;

public class RestoreValidatorTests
{
    private readonly FceRestoreDto _model = new(1, "00100001", 2022);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var serviceMock = Substitute.For<IFceService>();
        serviceMock.ExistsAsync(_model)
            .Returns(false);

        var validator = new FceRestoreValidator(serviceMock);

        // Act
        var result = await validator.TestValidateAsync(_model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NonUniqueFacilityYear_ReturnsAsInvalid()
    {
        // Arrange
        var serviceMock = Substitute.For<IFceService>();
        serviceMock.ExistsAsync(_model)
            .Returns(true);

        var validator = new FceRestoreValidator(serviceMock);

        // Act
        var result = await validator.TestValidateAsync(_model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
