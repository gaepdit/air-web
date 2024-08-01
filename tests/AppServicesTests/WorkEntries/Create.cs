using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.TestData.SampleData;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.WorkEntries;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 901;
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = SampleText.ValidEmail };
        var workEntry = new PermitRevocation(id);

        var workEntryManagerMock = Substitute.For<IWorkEntryManager>();
        workEntryManagerMock.Create(Arg.Any<WorkEntryType>(), Arg.Any<ApplicationUser?>())
            .Returns(workEntry);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(user);
        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(user);

        var notificationMock = Substitute.For<IAppNotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<CancellationToken>(),
                Arg.Any<object?[]>())
            .Returns(AppNotificationResult.SuccessResult());

        var facilityId = (FacilityId)"00100001";
        var facility = new Facility(facilityId);

        var facilityRepository = Substitute.For<IFacilityRepository>();
        facilityRepository.GetFacilityAsync(facilityId, Arg.Any<CancellationToken>())
            .Returns(facility);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            workEntryManagerMock, notificationMock, facilityRepository, userServiceMock,
            Substitute.For<IAuthorizationService>());

        var item = new PermitRevocationCreateDto
        {
            FacilityId = facilityId,
            Notes = SampleText.ValidName,
            ResponsibleStaffId = user.Id,
        };

        // Act
        var result = await appService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.AppNotificationResult.Should().NotBeNull();
        result.AppNotificationResult!.Success.Should().BeTrue();
        result.AppNotificationResult.FailureMessage.Should().BeEmpty();
        result.Id.Should().Be(id);
    }
}
