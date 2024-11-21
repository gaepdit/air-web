using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Compliance;

[Authorize(Policy = nameof(Policies.Staff))]
public class DownloadSearchModel(IComplianceSearchService searchService) : PageModel
{
    public IComplianceSearchDto Spec { get; private set; } = null!;
    public string Type { get; private set; } = string.Empty;
    public int ResultsCount { get; private set; }
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    // Work Entries
    public async Task<IActionResult> OnGetWorkEntriesAsync(WorkEntrySearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await searchService.CountWorkEntriesAsync(spec, token);
        Spec = spec;
        Type = "Work Entry";
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadWorkEntriesAsync(WorkEntrySearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        return Export("Compliance", await searchService.ExportWorkEntriesAsync(spec, token), spec.DeleteStatus == null);
    }

    // FCEs

    public async Task<IActionResult> OnGetFcesAsync(FceSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await searchService.CountFcesAsync(spec, token);
        Spec = spec;
        Type = "FCE";
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadFcesAsync(FceSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        return Export("FCE", await searchService.ExportFcesAsync(spec, token), spec.DeleteStatus == null);
    }

    // Common
    private FileStreamResult Export(string name, IReadOnlyList<IExportDto> exportList, bool removeLastColumn)
    {
        var excel = exportList.ToExcel(sheetName: $"{name} Search Results", removeLastColumn);
        var fileDownloadName = $"{name}_search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, ExcelContentType, fileDownloadName);
    }
}
