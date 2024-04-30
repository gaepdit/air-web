using MyApp.AppServices.Offices;
using MyApp.TestData.Constants;
using MyApp.WebApp.Api;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        // Arrange
        List<OfficeViewDto> officeList = [new OfficeViewDto(Guid.NewGuid(), TextData.ValidName, true)];

        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.GetListAsync(CancellationToken.None).Returns(officeList);
        
        var apiController = new OfficeApiController(serviceMock, Substitute.For<IAuthorizationService>());

        // Act
        var result = await apiController.ListOfficesAsync();

        // Assert
        result.Should().BeEquivalentTo(officeList);
    }
}
