namespace AirWeb.AppServices.Compliance.FacilitySearch;

public interface IFacilitySearchResult
{
    public string FacilityId { get; }
    public string? FacilityName { get; set; }
}
