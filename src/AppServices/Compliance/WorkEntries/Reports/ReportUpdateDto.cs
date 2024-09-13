namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public record ReportUpdateDto : ReportCommandDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
