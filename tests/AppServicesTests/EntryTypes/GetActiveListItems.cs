using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.UserServices;
using MyApp.Domain.Entities.EntryTypes;
using System.Linq.Expressions;

namespace AppServicesTests.EntryTypes;

public class GetActiveListItems
{
    [Test]
    public async Task GetAsListItems_ReturnsListOfListItems()
    {
        // Arrange
        var itemList = new List<EntryType>
        {
            new(Guid.Empty, "One"),
            new(Guid.Empty, "Two"),
        };

        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<Expression<Func<EntryType, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(itemList);

        var managerMock = Substitute.For<IEntryTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.GetAsListItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
