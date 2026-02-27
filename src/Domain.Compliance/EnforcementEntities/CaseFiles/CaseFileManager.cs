using AirWeb.Domain.Compliance.AuditPoints;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.DataExchange;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;

public sealed class CaseFileManager(ICaseFileRepository repository, IFacilityService facilityService)
    : ICaseFileManager
{
    public async Task<CaseFile> CreateAsync(FacilityId facilityId, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var caseFile = new CaseFile(repository.GetNextId(), facilityId, user);

        caseFile.AuditPoints.Add(CaseFileAuditPoint.Added(user));
        return caseFile;
    }

    public async Task AddEnforcementAction(CaseFile caseFile, EnforcementAction action, ApplicationUser? user)
    {
        caseFile.EnforcementActions.Add(action);
        caseFile.AuditPoints.Add(CaseFileAuditPoint.EnforcementActionAdded(action.ActionType, user));

        if (action is not DxActionEnforcementAction) return;

        if (caseFile.ActionNumber is null)
            caseFile.InitializeDataExchange(await facilityService
                .GetNextActionNumberAsync((FacilityId)action.FacilityId).ConfigureAwait(false));
        else
            caseFile.UpdateDataExchange();
    }

    public void Close(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.Close(user);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.Closed(user));
    }

    public void Reopen(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.Reopen(user);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.Reopened(user));
    }

    public void Delete(CaseFile caseFile, string? comment, ApplicationUser? user)
    {
        caseFile.Delete(comment, user);
        caseFile.DeleteDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.Deleted(user));
    }

    public void Restore(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.Undelete();
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.Restored(user));
    }

    public void Update(CaseFile caseFile, ApplicationUser? user)
    {
        caseFile.SetUpdater(user?.Id);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.Edited(user));
    }

    public void LinkComplianceEvent(CaseFile caseFile, ComplianceEvent entry, ApplicationUser? user)
    {
        caseFile.ComplianceEvents.Add(entry);
        entry.CaseFiles.Add(caseFile);

        caseFile.SetUpdater(user?.Id);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.ComplianceEventLinked(entry.Id, user));
    }

    public void UnlinkComplianceEvent(CaseFile caseFile, ComplianceEvent entry, ApplicationUser? user)
    {
        caseFile.ComplianceEvents.Remove(entry);
        entry.CaseFiles.Remove(caseFile);

        caseFile.SetUpdater(user?.Id);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.ComplianceEventUnlinked(entry.Id, user));
    }

    public void UpdatePollutantsAndPrograms(CaseFile caseFile, IEnumerable<string> pollutants,
        IEnumerable<AirProgram> airPrograms,
        ApplicationUser? user)
    {
        caseFile.PollutantIds.Clear();
        caseFile.PollutantIds.AddRange(pollutants);
        caseFile.AirPrograms.Clear();
        caseFile.AirPrograms.AddRange(airPrograms);

        caseFile.SetUpdater(user?.Id);
        caseFile.UpdateDataExchange();
        caseFile.AuditPoints.Add(CaseFileAuditPoint.PollutantsAndProgramsUpdated(user));
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
