using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public BaseWorkEntry Create(WorkEntryType type, ApplicationUser? user,
        ComplianceEventType? complianceEventType = null, NotificationType? notificationType = null)
    {
        var id = repository.GetNextId();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        BaseWorkEntry item = type switch
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

    public Comment CreateComment(string text, ApplicationUser? user) =>
        new()
        {
            Id = Guid.NewGuid(),
            Text = text,
            CommentBy = user,
            CommentedAt = DateTimeOffset.Now,
        };

    private static BaseComplianceEvent CreateComplianceEvent(ComplianceEventType? type, int? id) =>
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

    public void Close(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        baseWorkEntry.SetUpdater(user?.Id);
        baseWorkEntry.IsClosed = true;
        baseWorkEntry.ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        baseWorkEntry.ClosedBy = user;
    }

    public void Reopen(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        baseWorkEntry.SetUpdater(user?.Id);
        baseWorkEntry.IsClosed = false;
        baseWorkEntry.ClosedDate = null;
        baseWorkEntry.ClosedBy = null;
    }

    public void Delete(BaseWorkEntry baseWorkEntry, string? comment, ApplicationUser? user)
    {
        baseWorkEntry.SetDeleted(user?.Id);
        baseWorkEntry.DeletedBy = user;
        baseWorkEntry.DeleteComments = comment;
    }

    public void Restore(BaseWorkEntry baseWorkEntry)
    {
        baseWorkEntry.SetNotDeleted();
        baseWorkEntry.DeletedBy = null;
        baseWorkEntry.DeleteComments = null;
    }
}
