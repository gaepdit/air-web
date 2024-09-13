namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

public record AccUpdateDto : AccCommandDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
