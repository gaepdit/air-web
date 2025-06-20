using AirWeb.AppServices.CommonSearch;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public interface IWorkEntrySearchService
    : ISearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;
