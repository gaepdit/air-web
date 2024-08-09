using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.DataExport;

// TODO: Combine with search service.
public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token);

    Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(WorkEntrySearchDto spec,
        CancellationToken token);
}
