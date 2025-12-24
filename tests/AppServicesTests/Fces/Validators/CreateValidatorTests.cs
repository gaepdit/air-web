using AirWeb.AppServices.Compliance.Fces;
using AirWeb.Domain.ComplianceEntities.Fces;
using FluentValidation.TestHelper;
using IaipDataService.Facilities;

namespace AppServicesTests.Fces.Validators;

public class CreateValidatorTests
{
    private readonly FceCreateDto _model = new((FacilityId)"001-00001", "") { Year = DateTime.Now.Year };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IFceRepository>();
        repoMock.ExistsAsync((FacilityId)_model.FacilityId!, _model.Year)
            .Returns(false);

        var validator = new FceCreateValidator(repoMock);

        // Act
        var result = await validator.TestValidateAsync(_model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NonUniqueFacilityYear_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<IFceRepository>();
        repoMock.ExistsAsync((FacilityId)_model.FacilityId!, _model.Year)
            .Returns(true);

        var validator = new FceCreateValidator(repoMock);

        // Act
        var result = await validator.TestValidateAsync(_model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
