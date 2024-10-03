using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Search;

public interface IStandardSearchResult
{
    public Facility Facility { get; set; }
}
