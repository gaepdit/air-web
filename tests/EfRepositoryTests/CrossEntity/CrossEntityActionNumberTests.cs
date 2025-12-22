using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.CrossEntity;

public class CrossEntityActionNumberTests
{
    [Test]
    public async Task Insert_DifferentEntityTypesSameFacility_AssignsUniqueActionNumbers()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        var fceRepo = repositoryHelper.GetFceRepository();
        var caseFileRepo = repositoryHelper.GetCaseFileRepository();
        var workEntryRepo = repositoryHelper.GetWorkEntryRepository();
        
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        
        var fce = new Fce(null, facilityId, 2020, null);
        var caseFile = new CaseFile(null, facilityId, null);
        var report = new Report(null, facilityId, null);

        // Act - Insert all three entity types for the same facility
        await fceRepo.InsertAsync(fce);
        await caseFileRepo.InsertAsync(caseFile);
        await workEntryRepo.InsertAsync(report);

        // Assert - ActionNumbers should be unique across all entity types
        repositoryHelper.ClearChangeTracker();
        
        var savedFce = await repositoryHelper.Context.Set<Fce>()
            .FirstAsync(f => f.FacilityId == facilityId.ToString());
        var savedCaseFile = await repositoryHelper.Context.Set<CaseFile>()
            .FirstAsync(c => c.FacilityId == facilityId.ToString());
        var savedReport = await repositoryHelper.Context.Set<Report>()
            .FirstAsync(r => r.FacilityId == facilityId.ToString());

        // All should have different ActionNumbers
        savedFce.ActionNumber.Should().Be(1);
        savedCaseFile.ActionNumber.Should().Be(2);
        savedReport.ActionNumber.Should().Be(3);

        // Verify they're all different
        var actionNumbers = new[] { savedFce.ActionNumber, savedCaseFile.ActionNumber, savedReport.ActionNumber };
        actionNumbers.Distinct().Should().HaveCount(3);
    }

    [Test]
    public async Task Insert_AfterExistingCrossEntityRecords_ContinuesSequence()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        var fceRepo = repositoryHelper.GetFceRepository();
        var caseFileRepo = repositoryHelper.GetCaseFileRepository();
        var workEntryRepo = repositoryHelper.GetWorkEntryRepository();
        
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        
        // First, insert an FCE with ActionNumber 1
        var fce1 = new Fce(null, facilityId, 2020, null);
        await fceRepo.InsertAsync(fce1);
        
        repositoryHelper.ClearChangeTracker();
        var savedFce1 = await repositoryHelper.Context.Set<Fce>()
            .FirstAsync(f => f.FacilityId == facilityId.ToString());
        savedFce1.ActionNumber.Should().Be(1);
        
        // Now insert a CaseFile - it should get ActionNumber 2
        var caseFile = new CaseFile(null, facilityId, null);
        await caseFileRepo.InsertAsync(caseFile);
        
        repositoryHelper.ClearChangeTracker();
        var savedCaseFile = await repositoryHelper.Context.Set<CaseFile>()
            .FirstAsync(c => c.FacilityId == facilityId.ToString());
        savedCaseFile.ActionNumber.Should().Be(2);
        
        // Finally insert a Report - it should get ActionNumber 3
        var report = new Report(null, facilityId, null);
        await workEntryRepo.InsertAsync(report);
        
        repositoryHelper.ClearChangeTracker();
        var savedReport = await repositoryHelper.Context.Set<Report>()
            .FirstAsync(r => r.FacilityId == facilityId.ToString());
        savedReport.ActionNumber.Should().Be(3);
    }

    [Test]
    public async Task Insert_MultipleFacilities_AssignsIndependentSequences()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        var fceRepo = repositoryHelper.GetFceRepository();
        var caseFileRepo = repositoryHelper.GetCaseFileRepository();
        
        // Clear all tables after getting repositories
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityA = (FacilityId)"00100001";
        var facilityB = (FacilityId)"00100002";
        
        // Insert FCE for facility A (should get 1)
        var fceA = new Fce(null, facilityA, 2020, null);
        await fceRepo.InsertAsync(fceA);
        
        // Insert CaseFile for facility B (should also get 1, independent sequence)
        var caseFileB = new CaseFile(null, facilityB, null);
        await caseFileRepo.InsertAsync(caseFileB);
        
        // Insert CaseFile for facility A (should get 2, continuing A's sequence)
        var caseFileA = new CaseFile(null, facilityA, null);
        await caseFileRepo.InsertAsync(caseFileA);

        // Assert
        repositoryHelper.ClearChangeTracker();
        
        var savedFceA = await repositoryHelper.Context.Set<Fce>()
            .FirstAsync(f => f.FacilityId == facilityA.ToString());
        var savedCaseFileB = await repositoryHelper.Context.Set<CaseFile>()
            .FirstAsync(c => c.FacilityId == facilityB.ToString());
        var savedCaseFileA = await repositoryHelper.Context.Set<CaseFile>()
            .FirstAsync(c => c.FacilityId == facilityA.ToString());

        // Facility A should have sequence 1, 2
        savedFceA.ActionNumber.Should().Be(1);
        savedCaseFileA.ActionNumber.Should().Be(2);
        
        // Facility B should have sequence 1 (independent)
        savedCaseFileB.ActionNumber.Should().Be(1);
    }

    [Test]
    public async Task Insert_MixedEntityTypesInSingleTransaction_AssignsUniqueSequentialNumbers()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        var fceRepo = repositoryHelper.GetFceRepository();
        var caseFileRepo = repositoryHelper.GetCaseFileRepository();
        var workEntryRepo = repositoryHelper.GetWorkEntryRepository();
        
        // Clear all tables after getting repositories
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        
        // Create multiple entities of different types but don't save yet
        var fce1 = new Fce(null, facilityId, 2020, null);
        var fce2 = new Fce(null, facilityId, 2021, null);
        var caseFile1 = new CaseFile(null, facilityId, null);
        var report1 = new Report(null, facilityId, null);
        
        // Act - Add all entities to the context and save in one transaction
        repositoryHelper.Context.Fces.Add(fce1);
        repositoryHelper.Context.Fces.Add(fce2);
        repositoryHelper.Context.CaseFiles.Add(caseFile1);
        repositoryHelper.Context.Set<Report>().Add(report1);
        
        await repositoryHelper.Context.SaveChangesAsync();

        // Assert - All should have unique sequential ActionNumbers
        repositoryHelper.ClearChangeTracker();
        
        var allFces = await repositoryHelper.Context.Set<Fce>()
            .Where(f => f.FacilityId == facilityId.ToString())
            .OrderBy(f => f.ActionNumber)
            .ToListAsync();
        var allCaseFiles = await repositoryHelper.Context.Set<CaseFile>()
            .Where(c => c.FacilityId == facilityId.ToString())
            .ToListAsync();
        var allReports = await repositoryHelper.Context.Set<Report>()
            .Where(r => r.FacilityId == facilityId.ToString())
            .ToListAsync();

        // Collect all ActionNumbers
        var allActionNumbers = new List<ushort?>();
        allActionNumbers.AddRange(allFces.Select(f => f.ActionNumber));
        allActionNumbers.AddRange(allCaseFiles.Select(c => c.ActionNumber));
        allActionNumbers.AddRange(allReports.Select(r => r.ActionNumber));

        // All should be assigned
        allActionNumbers.Should().AllSatisfy(n => n.Should().NotBeNull());
        
        // All should be unique
        allActionNumbers.Distinct().Should().HaveCount(4);
        
        // Should be sequential: 1, 2, 3, 4
        allActionNumbers.OrderBy(n => n).Should().ContainInOrder((ushort)1, (ushort)2, (ushort)3, (ushort)4);
    }
}
