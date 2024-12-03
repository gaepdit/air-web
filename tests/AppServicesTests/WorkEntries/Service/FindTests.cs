using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AppServicesTests.WorkEntries.Service;

public class FindTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new PermitRevocation(902, new FacilityId(SampleText.ValidFacilityId));

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        repoMock.FindWithCommentsAsync<PermitRevocation>(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);
        repoMock.GetWorkEntryTypeAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(WorkEntryType.PermitRevocation);

        var facilityRepoMock = Substitute.For<IFacilityService>();
        facilityRepoMock.GetNameAsync(new FacilityId(item.FacilityId))
            .Returns(SampleText.ValidName);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IWorkEntryManager>(), facilityRepoMock, Substitute.For<ICommentService<int>>(),
            Substitute.For<IUserService>(), Substitute.For<IAppNotificationService>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }


    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IWorkEntryManager>(), Substitute.For<IFacilityService>(),
            Substitute.For<ICommentService<int>>(), Substitute.For<IUserService>(),
            Substitute.For<IAppNotificationService>());

        // Act
        var result = await appService.FindAsync(-1);

        // Assert
        result.Should().BeNull();
    }
}
