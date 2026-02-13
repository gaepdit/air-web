using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.Core.Entities;
using AirWeb.TestData.SampleData;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new Office(Guid.NewGuid(), SampleText.ValidName);

        var managerMock = Substitute.For<IOfficeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var appService = new OfficeService(AppServicesTestsSetup.Mapper!, Substitute.For<IOfficeRepository>(),
            managerMock, userServiceMock, Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.CreateAsync(SampleText.ValidName);

        // Assert
        result.Should().Be(item.Id);
    }
}
