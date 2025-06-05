﻿using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Users;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.TestData.SampleData;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var office = new Office(Guid.Empty, SampleText.ValidName);
        var itemList = new List<Office> { office };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(itemList);

        var appService = new OfficeService(AppServicesTestsSetup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new List<Office>());

        var appService = new OfficeService(AppServicesTestsSetup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
