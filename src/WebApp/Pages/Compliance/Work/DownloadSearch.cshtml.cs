﻿using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class DownloadSearchModel(ISearchResultsExportService searchResultsExportService) : PageModel
{
    public WorkEntrySearchDto Spec { get; private set; } = default!;
    public int ResultsCount { get; private set; }
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public async Task<IActionResult> OnGetAsync(WorkEntrySearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await searchResultsExportService.CountAsync(spec, token);
        Spec = spec;
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadAsync(WorkEntrySearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        var excel = (await searchResultsExportService.ExportSearchResultsAsync(spec, token))
            .ToExcel(sheetName: "Search Results", deleteLastColumn: spec.DeleteStatus == null);
        var fileDownloadName = $"search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, ExcelContentType, fileDownloadName: fileDownloadName);
    }
}
