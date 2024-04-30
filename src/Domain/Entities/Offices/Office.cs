namespace MyApp.Domain.Entities.Offices;

public class Office : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public Office() { }
    internal Office(Guid id, string name) : base(id, name) { }
}
