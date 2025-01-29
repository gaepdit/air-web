using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public interface IWorkEntrySearchService
    : IComplianceSearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;
