using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public WorkEntry Create(WorkEntryType type, ApplicationUser? user, NotificationType? notificationType = null)
    {
        var id = repository.GetNextId();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        WorkEntry item = type switch
        {
            WorkEntryType.AnnualComplianceCertification => new AnnualComplianceCertification(id),
            WorkEntryType.Inspection => new Inspection(id),
            WorkEntryType.Notification => new Notification(id,
                notificationType ??
                throw new ArgumentException("Missing notification type.", nameof(notificationType))),
            WorkEntryType.PermitRevocation => new PermitRevocation(id),
            WorkEntryType.Report => new Report(id),
            WorkEntryType.RmpInspection => new RmpInspection(id),
            WorkEntryType.SourceTestReview => new SourceTestReview(id),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };

        item.SetCreator(user?.Id);
        return item;
    }

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
