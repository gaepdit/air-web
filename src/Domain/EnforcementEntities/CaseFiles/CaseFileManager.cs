using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public class CaseFileManager(ICaseFileRepository repository, IFacilityService facilityService) : ICaseFileManager
{
    public async Task<CaseFile> Create(FacilityId facilityId, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        return new CaseFile(repository.GetNextId(), facilityId, user);
    }

    public void Close(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.SetUpdater(user?.Id);
        caseFile.Close(user);
    }

    public void Reopen(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.SetUpdater(user?.Id);
        caseFile.Reopen();
    }

    public void Delete(CaseFile caseFile, string? comment, ApplicationUser? user) =>
        caseFile.Delete(comment, user);

    public void Restore(CaseFile caseFile) => caseFile.Undelete();
}
