﻿using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AppServicesTests.WorkEntries.Service;

public class CreateTests
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

        var facilityRepository = Substitute.For<IFacilityService>();
        facilityRepository.GetAsync(facilityId, Arg.Any<CancellationToken>())
            .Returns(facility);

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            workEntryManagerMock, facilityRepository, Substitute.For<ICommentService<int>>(), userServiceMock,
            notificationMock);

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
