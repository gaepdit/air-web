using AirWeb.AppServices.Compliance.Comments;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Service;

public class FindTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new PermitRevocation(902, new FacilityId(SampleText.ValidFacilityId));

        var repoMock = Substitute.For<IComplianceWorkRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        repoMock.FindAsync<PermitRevocation>(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);
        repoMock.GetComplianceWorkTypeAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(ComplianceWorkType.PermitRevocation);

        var facilityRepoMock = Substitute.For<IFacilityService>();
        facilityRepoMock.GetNameAsync(new FacilityId(item.FacilityId))
            .Returns(SampleText.ValidName);

        var appService = new ComplianceWorkService(Setup.Mapper!, repoMock,
            Substitute.For<IComplianceWorkManager>(), facilityRepoMock, Substitute.For<ISourceTestService>(),
            Substitute.For<IComplianceWorkCommentService>(), Substitute.For<IUserService>(),
            Substitute.For<ICaseFileService>(),
            Substitute.For<IAppNotificationService>());

        // Act
        var result = await appService.FindAsync(item.Id, includeComments: false);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(item);
        result.Should().BeOfType<PermitRevocationViewDto>();
    }


    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IComplianceWorkRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var appService = new ComplianceWorkService(Setup.Mapper!, repoMock,
            Substitute.For<IComplianceWorkManager>(), Substitute.For<IFacilityService>(),
            Substitute.For<ISourceTestService>(), Substitute.For<IComplianceWorkCommentService>(),
            Substitute.For<IUserService>(), Substitute.For<ICaseFileService>(),
            Substitute.For<IAppNotificationService>());

        // Act
        var result = await appService.FindAsync(-1, includeComments: false);

        // Assert
        result.Should().BeNull();
    }
}
