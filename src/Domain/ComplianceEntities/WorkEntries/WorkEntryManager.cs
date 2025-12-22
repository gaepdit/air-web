using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public sealed class WorkEntryManager(IWorkEntryRepository repository, IFacilityService facilityService)
    : IWorkEntryManager
{
    public async Task<WorkEntry> CreateAsync(WorkEntryType type, FacilityId facilityId, ApplicationUser? user)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var id = repository.GetNextId();

        WorkEntry workEntry = type switch
        {
            WorkEntryType.AnnualComplianceCertification => new AnnualComplianceCertification(id, facilityId, user),
            WorkEntryType.Inspection => new Inspection(id, facilityId, user),
            WorkEntryType.Notification => new Notification(id, facilityId, user),
            WorkEntryType.PermitRevocation => new PermitRevocation(id, facilityId, user),
            WorkEntryType.Report => new Report(id, facilityId, user),
            WorkEntryType.RmpInspection => new RmpInspection(id, facilityId, user),
            WorkEntryType.SourceTestReview => new SourceTestReview(id, facilityId, user),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };

        if (workEntry is ComplianceEvent complianceEvent and not RmpInspection)
        {
            var actionNumber = await facilityService.GetNextActionNumberAsync(facilityId).ConfigureAwait(false);
            complianceEvent.ActionNumber = actionNumber;
            complianceEvent.DataExchangeStatus = DataExchangeStatus.I;
            complianceEvent.DataExchangeStatusDate = DateTimeOffset.Now;
        }

        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Added(user));
        return workEntry;
    }

    public void Update(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.SetUpdater(user?.Id);
        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Edited(user));
    }

    public void Close(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.Close(user);
        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Closed(user));
    }

    public void Reopen(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.Reopen(user);
        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Reopened(user));
    }

    public void Delete(WorkEntry workEntry, string? comment, ApplicationUser? user)
    {
        workEntry.Delete(comment, user);
        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Deleted(user));
    }

    public void Restore(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.Undelete();
        workEntry.AuditPoints.Add(WorkEntryAuditPoint.Restored(user));
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
