using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public sealed class ComplianceWorkManager(IComplianceWorkRepository repository, IFacilityService facilityService)
    : IComplianceWorkManager
{
    public async Task<ComplianceWork> CreateAsync(ComplianceWorkType type, FacilityId facilityId, ApplicationUser? user)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var id = repository.GetNextId();

        ComplianceWork complianceWork = type switch
        {
            ComplianceWorkType.AnnualComplianceCertification => new AnnualComplianceCertification(id, facilityId, user),
            ComplianceWorkType.Inspection => new Inspection(id, facilityId, user),
            ComplianceWorkType.Notification => new Notification(id, facilityId, user),
            ComplianceWorkType.PermitRevocation => new PermitRevocation(id, facilityId, user),
            ComplianceWorkType.Report => new Report(id, facilityId, user),
            ComplianceWorkType.RmpInspection => new RmpInspection(id, facilityId, user),
            ComplianceWorkType.SourceTestReview => new SourceTestReview(id, facilityId, user),
            _ => throw new ArgumentException("Invalid compliance work type.", nameof(type)),
        };

        if (complianceWork is ComplianceEvent ce and not RmpInspection)
            ce.InitiateDataExchange(await facilityService.GetNextActionNumberAsync(facilityId).ConfigureAwait(false));

        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Added(user));
        return complianceWork;
    }

    public void Update(ComplianceWork complianceWork, ApplicationUser? user)
    {
        complianceWork.SetUpdater(user?.Id);
        if (complianceWork is ComplianceEvent ce and not RmpInspection) ce.UpdateDataExchange();
        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Edited(user));
    }

    public void Close(ComplianceWork complianceWork, ApplicationUser? user)
    {
        complianceWork.Close(user);
        if (complianceWork is ComplianceEvent ce and not RmpInspection) ce.UpdateDataExchange();
        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Closed(user));
    }

    public void Reopen(ComplianceWork complianceWork, ApplicationUser? user)
    {
        complianceWork.Reopen(user);
        if (complianceWork is ComplianceEvent ce and not RmpInspection) ce.UpdateDataExchange();
        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Reopened(user));
    }

    public void Delete(ComplianceWork complianceWork, string? comment, ApplicationUser? user)
    {
        complianceWork.Delete(comment, user);
        if (complianceWork is ComplianceEvent ce and not RmpInspection) ce.DeleteDataExchange();
        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Deleted(user));
    }

    public void Restore(ComplianceWork complianceWork, ApplicationUser? user)
    {
        complianceWork.Undelete();
        if (complianceWork is ComplianceEvent ce and not RmpInspection) ce.UpdateDataExchange();
        complianceWork.AuditPoints.Add(ComplianceWorkAuditPoint.Restored(user));
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
