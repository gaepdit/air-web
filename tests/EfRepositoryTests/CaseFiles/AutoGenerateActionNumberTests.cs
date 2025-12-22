using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.CaseFiles;

public class AutoGenerateActionNumberTests
{
    [Test]
    public async Task Insert_WithoutActionNumber_GeneratesSequentialNumber()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        // Clear all entity types since ActionNumber is shared across all types
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var caseFile1 = new CaseFile(null, facilityId, null);
        var caseFile2 = new CaseFile(null, facilityId, null);
        var caseFile3 = new CaseFile(null, facilityId, null);

        // Act
        await repository.InsertAsync(caseFile1);
        await repository.InsertAsync(caseFile2);
        await repository.InsertAsync(caseFile3);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedCaseFiles = await repositoryHelper.Context.Set<CaseFile>()
            .Where(c => c.FacilityId == facilityId.ToString())
            .OrderBy(c => c.ActionNumber)
            .ToListAsync();

        savedCaseFiles.Should().HaveCount(3);
        savedCaseFiles[0].ActionNumber.Should().Be(1);
        savedCaseFiles[1].ActionNumber.Should().Be(2);
        savedCaseFiles[2].ActionNumber.Should().Be(3);
    }

    [Test]
    public async Task Insert_DifferentFacilities_StartsAtOneForEach()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        // Clear all entity types since ActionNumber is shared across all types
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityA = (FacilityId)"00100001";
        var facilityB = (FacilityId)"00100002";

        var caseFileA1 = new CaseFile(null, facilityA, null);
        var caseFileB1 = new CaseFile(null, facilityB, null);
        var caseFileA2 = new CaseFile(null, facilityA, null);

        // Act
        await repository.InsertAsync(caseFileA1);
        await repository.InsertAsync(caseFileB1);
        await repository.InsertAsync(caseFileA2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        
        var facilityACaseFiles = await repositoryHelper.Context.Set<CaseFile>()
            .Where(c => c.FacilityId == facilityA.ToString())
            .OrderBy(c => c.ActionNumber)
            .ToListAsync();

        var facilityBCaseFiles = await repositoryHelper.Context.Set<CaseFile>()
            .Where(c => c.FacilityId == facilityB.ToString())
            .ToListAsync();

        facilityACaseFiles.Should().HaveCount(2);
        facilityACaseFiles[0].ActionNumber.Should().Be(1);
        facilityACaseFiles[1].ActionNumber.Should().Be(2);

        facilityBCaseFiles.Should().HaveCount(1);
        facilityBCaseFiles[0].ActionNumber.Should().Be(1);
    }

    [Test]
    public async Task Insert_WithExistingActionNumber_PreservesIt()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        // Clear all entity types since ActionNumber is shared across all types
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var caseFile1 = new CaseFile(null, facilityId, null) { ActionNumber = 100 };

        // Act
        await repository.InsertAsync(caseFile1);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedCaseFile = await repositoryHelper.Context.Set<CaseFile>()
            .FirstOrDefaultAsync(c => c.FacilityId == facilityId.ToString());

        savedCaseFile.Should().NotBeNull();
        savedCaseFile!.ActionNumber.Should().Be(100);
    }

    [Test]
    public async Task Insert_AfterManualActionNumber_ContinuesSequence()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        // Clear all entity types since ActionNumber is shared across all types
        await repositoryHelper.ClearTableAsync<Fce, int>();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();
        await repositoryHelper.ClearTableAsync<WorkEntry, int>();

        var facilityId = (FacilityId)"00100001";
        var caseFile1 = new CaseFile(null, facilityId, null) { ActionNumber = 5 };
        var caseFile2 = new CaseFile(null, facilityId, null);

        // Act
        await repository.InsertAsync(caseFile1);
        await repository.InsertAsync(caseFile2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedCaseFiles = await repositoryHelper.Context.Set<CaseFile>()
            .Where(c => c.FacilityId == facilityId.ToString())
            .OrderBy(c => c.ActionNumber)
            .ToListAsync();

        savedCaseFiles.Should().HaveCount(2);
        savedCaseFiles[0].ActionNumber.Should().Be(5);
        savedCaseFiles[1].ActionNumber.Should().Be(6);
    }
}
