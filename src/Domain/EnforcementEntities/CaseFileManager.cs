using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities;

public class CaseFileManager(ICaseFileRepository repository, IFacilityService facilityService) : ICaseFileManager
{
    public async Task<CaseFile> CreateCaseFileAsync(FacilityId facilityId, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        return new CaseFile(repository.GetNextId(), facilityId, user);
    }

    public void Delete(CaseFile caseFile, string? comment, ApplicationUser? user) => caseFile.Delete(comment, user);
    public void Restore(CaseFile caseFile) => caseFile.Undelete();
}
