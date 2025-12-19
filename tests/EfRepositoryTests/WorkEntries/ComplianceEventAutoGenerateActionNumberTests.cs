using AirWeb.Domain.ComplianceEntities.WorkEntries;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.WorkEntries;

public class ComplianceEventAutoGenerateActionNumberTests
{
    [Test]
    public async Task Insert_WithoutActionNumber_GeneratesSequentialNumber()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var report1 = new Report(null, facilityId, null);
        var report2 = new Report(null, facilityId, null);
        var report3 = new Report(null, facilityId, null);

        // Act
        await repository.InsertAsync(report1);
        await repository.InsertAsync(report2);
        await repository.InsertAsync(report3);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedReports = await repositoryHelper.Context.Set<Report>()
            .Where(r => r.FacilityId == facilityId.ToString())
            .OrderBy(r => r.ActionNumber)
            .ToListAsync();

        savedReports.Should().HaveCount(3);
        savedReports[0].ActionNumber.Should().Be(1);
        savedReports[1].ActionNumber.Should().Be(2);
        savedReports[2].ActionNumber.Should().Be(3);
    }

    [Test]
    public async Task Insert_DifferentFacilities_StartsAtOneForEach()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityA = (FacilityId)"00100001";
        var facilityB = (FacilityId)"00100002";

        var reportA1 = new Report(null, facilityA, null);
        var reportB1 = new Report(null, facilityB, null);
        var reportA2 = new Report(null, facilityA, null);

        // Act
        await repository.InsertAsync(reportA1);
        await repository.InsertAsync(reportB1);
        await repository.InsertAsync(reportA2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        
        var facilityAReports = await repositoryHelper.Context.Set<Report>()
            .Where(r => r.FacilityId == facilityA.ToString())
            .OrderBy(r => r.ActionNumber)
            .ToListAsync();

        var facilityBReports = await repositoryHelper.Context.Set<Report>()
            .Where(r => r.FacilityId == facilityB.ToString())
            .ToListAsync();

        facilityAReports.Should().HaveCount(2);
        facilityAReports[0].ActionNumber.Should().Be(1);
        facilityAReports[1].ActionNumber.Should().Be(2);

        facilityBReports.Should().HaveCount(1);
        facilityBReports[0].ActionNumber.Should().Be(1);
    }

    [Test]
    public async Task Insert_WithExistingActionNumber_PreservesIt()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var report1 = new Report(null, facilityId, null) { ActionNumber = 100 };

        // Act
        await repository.InsertAsync(report1);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedReport = await repositoryHelper.Context.Set<Report>()
            .FirstOrDefaultAsync(r => r.FacilityId == facilityId.ToString());

        savedReport.Should().NotBeNull();
        savedReport!.ActionNumber.Should().Be(100);
    }

    [Test]
    public async Task Insert_AfterManualActionNumber_ContinuesSequence()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var report1 = new Report(null, facilityId, null) { ActionNumber = 5 };
        var report2 = new Report(null, facilityId, null);

        // Act
        await repository.InsertAsync(report1);
        await repository.InsertAsync(report2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedReports = await repositoryHelper.Context.Set<Report>()
            .Where(r => r.FacilityId == facilityId.ToString())
            .OrderBy(r => r.ActionNumber)
            .ToListAsync();

        savedReports.Should().HaveCount(2);
        savedReports[0].ActionNumber.Should().Be(5);
        savedReports[1].ActionNumber.Should().Be(6);
    }
}
