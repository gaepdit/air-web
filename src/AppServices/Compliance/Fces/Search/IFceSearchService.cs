using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public interface IFceSearchService : ISearchService<FceSearchDto, FceSearchResultDto, FceExportDto>;
