namespace AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

public interface IWorkEntryCreateDto : IWorkEntryCommandDto
{
    public string? FacilityId { get; }
}
