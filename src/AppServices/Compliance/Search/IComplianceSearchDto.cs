namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: See if these will work with enforcement as well.

public interface IComplianceSearchDto
{
    SortBy Sort { get; }
    DeleteStatus? DeleteStatus { get; set; }
    IDictionary<string, string?> AsRouteValues();
}
