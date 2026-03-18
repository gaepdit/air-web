using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Core.Offices;

public class FindForUpdateTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var office = new Office(id, SampleText.ValidName);

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns(office);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(), cache,
            Substitute.For<ILogger<OfficeService>>());

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

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(), cache,
            Substitute.For<ILogger<OfficeService>>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
