namespace MyApp.Domain.Entities.EntryTypes;

public class EntryType : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public EntryType() { }
    internal EntryType(Guid id, string name) : base(id, name) { }
}
