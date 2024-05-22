using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.WorkEntries;

public class WorkEntryManager(IWorkEntryRepository repository) : IWorkEntryManager
{
    public BaseWorkEntry Create(WorkEntryType type, ApplicationUser? user)
    {
        BaseWorkEntry item =default!;

        if (type == WorkEntryType.Notification)
            item = new Notification(repository.GetNextId()) { ReceivedBy = user };

        item.SetCreator(user?.Id);
        return item;
    }

    public EntryAction CreateEntryAction(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        var entryAction = new EntryAction(Guid.NewGuid(), baseWorkEntry);
        entryAction.SetCreator(user?.Id);
        return entryAction;
    }

    public void Close(BaseWorkEntry baseWorkEntry, string? comment, ApplicationUser? user)
    {
        baseWorkEntry.SetUpdater(user?.Id);
        baseWorkEntry.Status = WorkEntryStatus.Closed;
        baseWorkEntry.Closed = true;
        baseWorkEntry.ClosedDate = DateTime.Now;
        baseWorkEntry.ClosedBy = user;
        baseWorkEntry.ClosedComments = comment;
    }

    public void Reopen(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        baseWorkEntry.SetUpdater(user?.Id);
        baseWorkEntry.Status = WorkEntryStatus.Open;
        baseWorkEntry.Closed = false;
        baseWorkEntry.ClosedDate = null;
        baseWorkEntry.ClosedBy = null;
        baseWorkEntry.ClosedComments = null;
    }

    public void Delete(BaseWorkEntry baseWorkEntry, string? comment, ApplicationUser? user)
    {
        baseWorkEntry.SetDeleted(user?.Id);
        baseWorkEntry.DeletedBy = user;
        baseWorkEntry.DeleteComments = comment;
    }

    public void Restore(BaseWorkEntry baseWorkEntry, ApplicationUser? user)
    {
        baseWorkEntry.SetNotDeleted();
        baseWorkEntry.DeleteComments = null;
    }
}
