using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.CaseFileCommand;
using AirWeb.Core.Entities;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.SampleData;
using AutoMapper;
using IaipDataService.Facilities;

namespace AppServicesTests.Enforcement.CaseFiles;

public class CreateCaseFileTests
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
            .Returns(AppNotificationResult.Success());

        var caseFileService = new CaseFileService(
            Substitute.For<IMapper>(), caseFileRepositoryMock, caseFileManager,
            Substitute.For<IComplianceWorkRepository>(), Substitute.For<ICaseFileCommentService>(),
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
        result.HasWarning.Should().BeFalse();
        result.Id.Should().Be(id);
    }
}
