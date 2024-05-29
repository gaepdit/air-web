using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.UserServices;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.PermitRevocations;
using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using AirWeb.TestData.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.WorkEntries;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 901;
        var user = new ApplicationUser { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };

        var workEntry = new PermitRevocation(id);
        var workEntryManagerMock = Substitute.For<IWorkEntryManager>();

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(user);

        workEntryManagerMock.Create(Arg.Any<WorkEntryType>(), Arg.Any<ApplicationUser?>())
            .Returns(workEntry);

        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(user);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<CancellationToken>(),
                Arg.Any<object?[]>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            workEntryManagerMock, notificationMock, Substitute.For<IFacilityRepository>(), userServiceMock,
            Substitute.For<IAuthorizationService>());

        var item = new PermitRevocationCreateDto { Notes = TextData.Phrase };

        // Act
        var result = await appService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.NotificationResult.Should().NotBeNull();
        result.NotificationResult!.Success.Should().BeTrue();
        result.NotificationResult.FailureMessage.Should().BeEmpty();
        result.Id.Should().Be(id);
    }
}
