using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public record ReportUpdateDto : ReportCommandDto, IIsClosedAndIsDeleted
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
