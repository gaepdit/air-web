using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public interface IWorkEntrySearchService
    : IComplianceSearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;
