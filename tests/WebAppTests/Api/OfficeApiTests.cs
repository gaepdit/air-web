using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.TestData.SampleData;
using AirWeb.WebApp.Api;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        // Arrange
        List<OfficeViewDto> officeList =
        [
            new OfficeViewDto { Id = Guid.NewGuid(), Name = SampleText.ValidName, Active = true },
        ];

        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.GetListAsync(CancellationToken.None).Returns(officeList);

        var apiController = new OfficeApiController(serviceMock, Substitute.For<IAuthorizationService>());

        // Act
        var result = await apiController.ListOfficesAsync();

        // Assert
        result.Should().BeEquivalentTo(officeList);
    }
}
