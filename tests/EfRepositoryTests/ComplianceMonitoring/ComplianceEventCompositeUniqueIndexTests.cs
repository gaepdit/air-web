using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.ComplianceMonitoring;

public class ComplianceEventCompositeUniqueIndexTests
{
    private readonly FacilityId _facilityId = (FacilityId)"00199999";

    [Test]
    public async Task Insert_DifferentFacilitiesSameActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetComplianceWorkRepository();
        await repositoryHelper.ClearTableAsync<ComplianceWork, int>();

        var report1 = new Report(null, (FacilityId)"00199998") { ActionNumber = 1 };
        var report2 = new Report(null, (FacilityId)"00199999") { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(report1);
        await repository.InsertAsync(report2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<Report>().CountAsync(f => f.ActionNumber == 1)).Should().Be(2);
    }

    [Test]
    public async Task Insert_SameFacilitySameActionNumber_Fails()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        var repository = repositoryHelper.GetComplianceWorkRepository();
        await repositoryHelper.ClearTableAsync<ComplianceWork, int>();

        var report1 = new Report(null, _facilityId) { ActionNumber = 1 };
        var report2 = new Report(null, _facilityId) { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(report1);
        var action = async () => await repository.InsertAsync(report2);

        // Assert - should throw DbUpdateException due to unique constraint violation
        await action.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task Insert_SameFacilityDifferentActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetComplianceWorkRepository();
        await repositoryHelper.ClearTableAsync<ComplianceWork, int>();

        var report1 = new Report(null, _facilityId) { ActionNumber = 1 };
        var report2 = new Report(null, _facilityId) { ActionNumber = 2 };

        // Act
        await repository.InsertAsync(report1);
        await repository.InsertAsync(report2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<Report>()
            .CountAsync(f => f.FacilityId == _facilityId.ToString())).Should().Be(2);
    }

    [Test]
    public async Task Insert_NullActionNumbers_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetComplianceWorkRepository();
        await repositoryHelper.ClearTableAsync<ComplianceWork, int>();

        var report1 = new Report(null, _facilityId) { ActionNumber = null };
        var report2 = new Report(null, _facilityId) { ActionNumber = null };

        // Act
        await repository.InsertAsync(report1);
        await repository.InsertAsync(report2);

        // Assert - both should be saved successfully (null values don't violate unique constraint)
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<Report>()
            .CountAsync(f => f.FacilityId == _facilityId.ToString() && f.ActionNumber == null)).Should().Be(2);
    }
}
