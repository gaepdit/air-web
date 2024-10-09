namespace IaipDataService.Structs;

// FUTURE: Change to DateOnly when this Dapper issue is fixed and DateOnly is supported:
// https://github.com/DapperLib/Dapper/issues/2072
public readonly record struct DateRange(DateTime StartDate, DateTime? EndDate);
