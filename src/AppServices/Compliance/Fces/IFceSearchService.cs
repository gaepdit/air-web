using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceSearchService : IComplianceSearchService<FceSearchDto, FceSearchResultDto, FceExportDto>;
