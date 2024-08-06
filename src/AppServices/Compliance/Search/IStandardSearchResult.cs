namespace AirWeb.AppServices.Compliance.Search;

public interface IStandardSearchResult
{
    public string FacilityId { get; }
    public string FacilityName { get; set; }
}
