namespace AirWeb.AppServices.CommonSearch;

public interface ISearchResult
{
    public string FacilityId { get; }
    public string? FacilityName { get; set; }
}
