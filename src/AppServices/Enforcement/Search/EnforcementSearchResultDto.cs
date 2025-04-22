using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.AppServices.Enforcement.Search
{
    public record EnforcementSearchResultDto
    {
        public required string FacilityId { get; init; }
        public CaseFileStatus CaseFileStatus { get; init; }
        public DateOnly DiscoveryDate { get; init; }

    }
}
