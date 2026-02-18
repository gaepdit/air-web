using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AppServicesTests.Sbeap.TestData;

namespace AppServicesTests.Sbeap.Agencies;

public class GetListTests
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var agency = new Agency(Guid.Empty, Constants.ValidName);
        var itemList = new List<Agency> { agency };
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(itemList);
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Setup.Mapper!,
            Substitute.For<IUserService>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(new List<Agency>());
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Setup.Mapper!,
            Substitute.For<IUserService>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEmpty();
    }
}
