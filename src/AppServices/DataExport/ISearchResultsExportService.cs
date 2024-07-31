using AirWeb.AppServices.Compliance.Search;

namespace AirWeb.AppServices.DataExport;

public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(ComplianceSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(ComplianceSearchDto spec,
        CancellationToken token);
}
