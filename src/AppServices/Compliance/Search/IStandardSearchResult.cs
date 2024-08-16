using AirWeb.AppServices.ExternalEntities.Facilities;

namespace AirWeb.AppServices.Compliance.Search;

public interface IStandardSearchResult
{
    public FacilityViewDto Facility { get; set; }
}
