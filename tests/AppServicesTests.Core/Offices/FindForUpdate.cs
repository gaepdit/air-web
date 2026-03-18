using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Core.Offices;

public class FindForUpdate
{
    private static OfficeService GetService(IOfficeRepository repoMock)
    {
        var userServiceMock = Substitute.For<IUserService>();
        var logger = Substitute.For<ILogger<OfficeService>>();
        using var cache = Substitute.For<IMemoryCache>();
        return new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(), userServiceMock,
            Substitute.For<IAuthorizationService>(), cache, logger);
    }

    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var office = new Office(id, SampleText.ValidName);

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns(office);

        var appService = GetService(repoMock);

        // Act
        var result = await appService.FindForUpdateAsync(id);

        // Assert
        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Office?)null);

        var appService = GetService(repoMock);

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
