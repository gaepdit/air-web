using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesCoreTests.Offices;

public class GetStaff
{
    [Test]
    public async Task WhenOfficeExists_ReturnsViewDtoList()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = SampleText.ValidName,
            FamilyName = SampleText.NewValidName,
            Email = SampleText.ValidEmail,
            Active = false,
        };

        var itemList = new List<ApplicationUser> { user };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetStaffMembersListAsync(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(itemList);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.GetStaffAsListItemsAsync(Guid.NewGuid());

        // Assert
        result.Should().ContainSingle(e =>
            string.Equals(e.Id, user.Id, StringComparison.Ordinal) &&
            string.Equals(e.Name, user.SortableNameWithInactive, StringComparison.Ordinal));
    }
}
