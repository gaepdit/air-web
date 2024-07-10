using AirWeb.AppServices.DomainEntities.Fces;
using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using FluentValidation.TestHelper;

namespace AppServicesTests.Fces;

public class CreateValidator
{
    private readonly FceCreateDto _model = new() { FacilityId = (FacilityId)"001-00001", Year = 2000 };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<IFceRepository>();
        repoMock.ExistsAsync(_model.FacilityId!, _model.Year)
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
        repoMock.ExistsAsync(_model.FacilityId!, _model.Year)
            .Returns(true);

        var validator = new FceCreateValidator(repoMock);

        // Act
        var result = await validator.TestValidateAsync(_model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
