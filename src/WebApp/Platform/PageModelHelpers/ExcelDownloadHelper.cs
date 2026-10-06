using AirWeb.AppServices.Core.DataExport;

namespace AirWeb.WebApp.Platform.PageModelHelpers;

public static class ExcelDownloadHelper
{
    public static FileStreamResult ExcelFile<T>(this PageModel pageModel, IEnumerable<T> records,
        string sheetName, string fileName)
    {
        var fileDownloadName = $"{fileName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
        var excel = records.ToExcel(sheetName);
        return pageModel.File(excel, DataExportUtilities.ExcelContentType, fileDownloadName);
    }
}
