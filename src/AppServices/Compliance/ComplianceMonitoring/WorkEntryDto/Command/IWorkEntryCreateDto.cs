namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

public interface IWorkEntryCreateDto : IWorkEntryCommandDto
{
    public string? FacilityId { get; }
}
