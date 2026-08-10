using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;

public record NotificationCreateDto : NotificationCommandDto, IComplianceWorkCreateDto
{
    public string? FacilityId { get; set; }
    public int? CaseFileId => null;
}
