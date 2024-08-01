using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public WorkEntry Create(WorkEntryType type, ApplicationUser? user,
        ComplianceEventType? complianceEventType = null, NotificationType? notificationType = null)
    {
        var id = repository.GetNextId();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        WorkEntry item = type switch
        {
            WorkEntryType.Notification => new Notification(id,
                notificationType ??
                throw new ArgumentException("Missing notification type.", nameof(notificationType))),
            WorkEntryType.PermitRevocation => new PermitRevocation(id),
            WorkEntryType.ComplianceEvent => CreateComplianceEvent(complianceEventType, id),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };

        item.SetCreator(user?.Id);
        return item;
    }

    private static ComplianceEvent CreateComplianceEvent(ComplianceEventType? type, int? id) =>
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        type switch
        {
            ComplianceEventType.Inspection => new Inspection(id),
            ComplianceEventType.Report => new Report(id),
            ComplianceEventType.RmpInspection => new RmpInspection(id),
            ComplianceEventType.AnnualComplianceCertification => new AnnualComplianceCertification(id),
            ComplianceEventType.SourceTestReview => new SourceTestReview(id),
            null => throw new ArgumentException("Missing compliance event type.", nameof(type)),
            _ => throw new ArgumentException("Invalid compliance event type.", nameof(type)),
        };

    public void Close(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.SetUpdater(user?.Id);
        workEntry.IsClosed = true;
        workEntry.ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        workEntry.ClosedBy = user;
    }

    public void Reopen(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.SetUpdater(user?.Id);
        workEntry.IsClosed = false;
        workEntry.ClosedDate = null;
        workEntry.ClosedBy = null;
    }

    public void Delete(WorkEntry workEntry, string? comment, ApplicationUser? user)
    {
        workEntry.SetDeleted(user?.Id);
        workEntry.DeletedBy = user;
        workEntry.DeleteComments = comment;
    }

    public void Restore(WorkEntry workEntry)
    {
        workEntry.SetNotDeleted();
        workEntry.DeletedBy = null;
        workEntry.DeleteComments = null;
    }
}
