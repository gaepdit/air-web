using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Core.Offices;

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

        var appService = new OfficeService(Setup.Mapper!, Substitute.For<IOfficeRepository>(),
            managerMock, userServiceMock, Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.CreateAsync(SampleText.ValidName);

        // Assert
        result.Should().Be(item.Id);
    }
}
