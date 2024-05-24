using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public BaseWorkEntry CreateWorkEntry(WorkEntryType type, ApplicationUser? user)
    {
        var id = repository.GetNextId();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        BaseWorkEntry item = type switch
        {
            WorkEntryType.Notification => new Notification(id),
            WorkEntryType.PermitRevocation => new PermitRevocation(id),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };

        item.SetCreator(user?.Id);
        return item;
    }

    public BaseComplianceEvent CreateComplianceEvent(ComplianceEventType type, ApplicationUser? user)
    {
        var id = repository.GetNextId();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        BaseComplianceEvent item = type switch
        {
            ComplianceEventType.Inspection => new Inspection(id),
            ComplianceEventType.Report => new Report(id),
            ComplianceEventType.RmpInspection => new RmpInspection(id),
            ComplianceEventType.AnnualComplianceCertification => new AnnualComplianceCertification(id),
            ComplianceEventType.SourceTestReview => new SourceTestReview(id),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };

        item.SetCreator(user?.Id);
        return item;
    }

    public void Close(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        baseWorkEntry.SetUpdater(user?.Id);
        baseWorkEntry.IsClosed = true;
        baseWorkEntry.ClosedDate = DateTimeOffset.Now;
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
