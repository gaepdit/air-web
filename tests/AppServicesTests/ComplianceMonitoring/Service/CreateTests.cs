using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;

namespace AppServicesTests.ComplianceMonitoring.Service;

public class CreateTests
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        const int id = 901;
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = SampleText.ValidEmail };
        var facilityId = (FacilityId)"00100001";
        var work = new PermitRevocation(id, facilityId);

        var complianceWorkManagerMock = Substitute.For<IComplianceWorkManager>();
        complianceWorkManagerMock.CreateAsync(Arg.Any<ComplianceWorkType>(), Arg.Any<FacilityId>(),
                Arg.Any<ApplicationUser?>())
            .Returns(work);

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
            .Returns(AppNotificationResult.Success());

        var service = new ComplianceWorkService(AppServicesTestsSetup.Mapper!,
            Substitute.For<IComplianceWorkRepository>(),
            complianceWorkManagerMock, Substitute.For<IFacilityService>(), Substitute.For<ISourceTestService>(),
            Substitute.For<ICommentService<int>>(), userServiceMock, notificationMock);

        var item = new PermitRevocationCreateDto
        {
            FacilityId = facilityId,
            Notes = SampleText.ValidName,
            ResponsibleStaffId = user.Id,
        };

        // Act
        var result = await service.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.HasWarning.Should().BeFalse();
        result.WarningMessage.Should().BeNull();
        result.Id.Should().Be(id);
    }
}
