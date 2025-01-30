using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public interface IFceSearchService : IComplianceSearchService<FceSearchDto, FceSearchResultDto, FceExportDto>;
