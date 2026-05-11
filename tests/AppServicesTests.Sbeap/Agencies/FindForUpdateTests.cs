using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AppServicesTests.Sbeap.TestData;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Sbeap.Agencies;

public class FindForUpdateTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var agency = new Agency(Guid.Empty, Constants.ValidName);

        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindAsync(agency.Id, Arg.Any<CancellationToken>()).Returns(agency);

        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), Setup.FakeCache!, Substitute.For<ILogger<AgencyService>>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        //Assert
        result.Should().BeEquivalentTo(agency);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = Guid.Empty;

        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((Agency?)null);

        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Substitute.For<IMapper>(),
            Substitute.For<IUserService>(), Setup.FakeCache!, Substitute.For<ILogger<AgencyService>>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        //Assert
        result.Should().BeNull();
    }
}
