﻿using ClosedXML.Excel;

namespace AirWeb.AppServices.DataExport;

public static class DataExportUtilities
{
    /// <summary>
    /// Creates an Excel spreadsheet from <see cref="IEnumerable{T}"/> records. 
    /// </summary>
    /// <param name="records">The records to add to the spreadsheet.</param>
    /// <param name="sheetName">A name for the worksheet.</param>
    /// <param name="removeLastColumn">A flag indicating whether to remove the final column in the table
    /// (the column showing deletion status).</param>
    /// <typeparam name="T">The type of the records being inserted.</typeparam>
    /// <returns>A <see cref="MemoryStream"/> containing an Excel spreadsheet with a worksheet named
    /// <paramref name="sheetName"/> containing the data in <paramref name="records"/> as a table.</returns>
    public static MemoryStream ToExcel<T>(this IEnumerable<T> records, string sheetName,
        bool removeLastColumn)
    {
        using var xlWorkbook = new XLWorkbook();

        var xlWorksheet = xlWorkbook.AddWorksheet(sheetName);
        var xlTable = xlWorksheet.Cell(row: 1, column: 1).InsertTable(records);
        if (removeLastColumn) xlTable.Column(xlTable.Columns().Count()).Delete();
        xlTable.Cells().Style
            .Alignment.SetWrapText(true)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Top);
        xlWorksheet.Columns().AdjustToContents(minWidth: 8d, maxWidth: 80d);

        var memoryStream = new MemoryStream();
        xlWorkbook.SaveAs(memoryStream);
        memoryStream.Position = 0;

        return memoryStream;
    }
}
