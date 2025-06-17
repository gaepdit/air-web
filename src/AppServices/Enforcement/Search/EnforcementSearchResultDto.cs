using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.AppServices.Enforcement.Search
{
    public record EnforcementSearchResultDto
    {
        public int Id { get; init; }
        public required string FacilityId { get; init; }
        public string? FacilityName { get; set; }
        public CaseFileStatus CaseFileStatus { get; init; }
        public DateOnly? DiscoveryDate { get; init; }
        public DateOnly? DayZero { get; init; }
        public StaffViewDto? ResponsibleStaff { get; init; }

    }
}
