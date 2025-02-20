using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using AirWeb.TestData.SampleData;
using AutoMapper;
using IaipDataService.Facilities;

namespace AppServicesTests.Enforcement.CaseFiles;

public class CreateTests
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 901;
        var facilityId = (FacilityId)"00100001";

        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = SampleText.ValidEmail };
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns(user);
        userServiceMock.FindUserAsync(user.Id).Returns(user);

        var caseFileRepositoryMock = Substitute.For<ICaseFileRepository>();
        caseFileRepositoryMock.GetNextId().Returns(id);

        var facilityServiceMock = Substitute.For<IFacilityService>();
        facilityServiceMock.ExistsAsync(facilityId).Returns(true);

        var caseFileManager = new CaseFileManager(caseFileRepositoryMock, facilityServiceMock);

        var notificationMock = Substitute.For<IAppNotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<CancellationToken>(),
                Arg.Any<object?[]>())
            .Returns(AppNotificationResult.SuccessResult());

        var caseFileService = new CaseFileService(
            Substitute.For<IMapper>(), caseFileRepositoryMock, caseFileManager,
            Substitute.For<IWorkEntryRepository>(), Substitute.For<ICommentService<int>>(),
            Substitute.For<IFacilityService>(), userServiceMock, notificationMock);
        var item = new CaseFileCreateDto
        {
            FacilityId = facilityId,
            Notes = SampleText.ValidName,
            ResponsibleStaffId = user.Id,
        };

        // Act
        var result = await caseFileService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.AppNotificationResult!.Success.Should().BeTrue();
        result.Id.Should().Be(id);
    }
}
