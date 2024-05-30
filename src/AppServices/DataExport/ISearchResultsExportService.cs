using AirWeb.AppServices.WorkEntries.Search;

namespace AirWeb.AppServices.DataExport;

public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token);

    Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(WorkEntrySearchDto spec,
        CancellationToken token);
}
