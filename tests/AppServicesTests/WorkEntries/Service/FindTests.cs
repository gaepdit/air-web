using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.WorkEntries.Service;

public class FindTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new PermitRevocation(902) { Facility = new Facility(SampleText.ValidFacilityId) };

        var repoMock = Substitute.For<IWorkEntryRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        repoMock.FindWithCommentsAsync<PermitRevocation>(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);
        repoMock.GetWorkEntryTypeAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(WorkEntryType.PermitRevocation);

        var facilityRepoMock = Substitute.For<IFacilityRepository>();
        facilityRepoMock.GetFacilityAsync(item.Facility.Id)
            .Returns(item.Facility);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IWorkEntryManager>(), Substitute.For<IAppNotificationService>(),
            facilityRepoMock, Substitute.For<IUserService>());

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

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            Substitute.For<IWorkEntryManager>(), Substitute.For<IAppNotificationService>(),
            Substitute.For<IFacilityRepository>(), Substitute.For<IUserService>());

        // Act
        var result = await appService.FindAsync(-1);

        // Assert
        result.Should().BeNull();
    }
}
