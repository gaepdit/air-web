using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public WorkEntry Create(WorkEntryType type, ApplicationUser? user)
    {
        var id = repository.GetNextId();

        return type switch
        {
            WorkEntryType.AnnualComplianceCertification => new AnnualComplianceCertification(id, user),
            WorkEntryType.Inspection => new Inspection(id, user),
            WorkEntryType.Notification => new Notification(id, user),
            WorkEntryType.PermitRevocation => new PermitRevocation(id, user),
            WorkEntryType.Report => new Report(id, user),
            WorkEntryType.RmpInspection => new RmpInspection(id, user),
            WorkEntryType.SourceTestReview => new SourceTestReview(id, user),
            _ => throw new ArgumentException("Invalid work entry type.", nameof(type)),
        };
    }

    public void Close(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.SetUpdater(user?.Id);
        workEntry.Close(user);
    }

    public void Reopen(WorkEntry workEntry, ApplicationUser? user)
    {
        workEntry.SetUpdater(user?.Id);
        workEntry.Reopen();
    }

    public void Delete(WorkEntry workEntry, string? comment, ApplicationUser? user) => workEntry.Delete(comment, user);

    public void Restore(WorkEntry workEntry) => workEntry.Undelete();
}
