using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Notifications;
using MyApp.AppServices.UserServices;
using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.CommandDto;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.Domain.Identity;
using MyApp.TestData.Constants;

namespace AppServicesTests.WorkEntries;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new ApplicationUser { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };
        var workEntry = new WorkEntry(id) { ReceivedBy = user };

        var workEntryManagerMock = Substitute.For<IWorkEntryManager>();
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(user);

        workEntryManagerMock.Create(Arg.Any<ApplicationUser?>())
            .Returns(workEntry);

        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(user);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<WorkEntry>(),
                Arg.Any<CancellationToken>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            Substitute.For<IEntryTypeRepository>(), workEntryManagerMock, notificationMock, userServiceMock,
            Substitute.For<IAuthorizationService>());

        var item = new WorkEntryCreateDto { EntryTypeId = Guid.Empty, Notes = TextData.Phrase };

        // Act
        var result = await appService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.HasWarnings.Should().BeFalse();
        result.WorkEntryId.Should().Be(id);
    }
}
