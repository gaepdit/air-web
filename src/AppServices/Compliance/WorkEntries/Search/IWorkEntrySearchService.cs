using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public interface IWorkEntrySearchService
    : ISearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;
