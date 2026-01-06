namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public interface IWorkEntryCreateDto : IWorkEntryCommandDto
{
    public string? FacilityId { get; }
}
