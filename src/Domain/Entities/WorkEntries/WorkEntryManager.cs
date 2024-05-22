using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public BaseWorkEntry Create(WorkEntryType type, ApplicationUser? user)
    {
        var id = repository.GetNextId();

        BaseWorkEntry item = type switch
        {
            WorkEntryType.Notification => new Notification(id),
            WorkEntryType.Inspection => new Inspection(id),
            WorkEntryType.Report => new Report(id),
            WorkEntryType.PermitRevocation => new PermitRevocation(id),
            WorkEntryType.RmpInspection => new RmpInspection(id),
            WorkEntryType.AnnualComplianceCertification => new AnnualComplianceCertification(id),
            WorkEntryType.PerformanceTestReview => new PerformanceTestReview(id),
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
        baseWorkEntry.DeleteComments = null;
    }
}
