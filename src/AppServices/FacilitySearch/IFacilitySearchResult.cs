namespace AirWeb.AppServices.FacilitySearch;

public interface IFacilitySearchResult
{
    public string FacilityId { get; }
    public string? FacilityName { get; set; }
}
