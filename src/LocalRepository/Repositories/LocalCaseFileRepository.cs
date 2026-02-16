using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;
using AirWeb.TestData.Enforcement;
using IaipDataService.Facilities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalCaseFileRepository : BaseRepositoryWithMapping<CaseFile, int>, ICaseFileRepository
{
    public LocalCaseFileRepository() : base(CaseFileData.GetData) => _ = EnforcementActionData.GetData;

    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(caseFile => caseFile.Id).Max() + 1;

    public async Task<CaseFile?> FindWithDetailsAsync(int id, CancellationToken token = default)
    {
        var caseFile = await FindAsync(id, token: token).ConfigureAwait(false);
        caseFile?.Comments.RemoveAll(comment => comment.IsDeleted);
        return caseFile;
    }

    public Task<ViolationType?> GetViolationTypeAsync(string? code, CancellationToken token = default) =>
        Task.FromResult(ViolationTypeData.GetViolationType(code));

    // Pollutants & Air Programs
    public Task<IEnumerable<Pollutant>> GetPollutantsAsync(int id, CancellationToken token = default) =>
        Task.FromResult<IEnumerable<Pollutant>>(Items.Single(caseFile => caseFile.Id.Equals(id)).GetPollutants());

    public Task<IEnumerable<AirProgram>> GetAirProgramsAsync(int id, CancellationToken token = default) =>
        Task.FromResult<IEnumerable<AirProgram>>(Items.Single(caseFile => caseFile.Id.Equals(id)).AirPrograms);
}
