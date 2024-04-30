namespace MyApp.AppServices.WorkEntries.CommandDto;

public interface IWorkEntryCommandDto
{
    public Guid EntryTypeId { get; }
    public string Notes { get; }
}
