namespace AirWeb.Domain.Reports;

public static class ReportsConstants
{
    public const string ConfidentialInfoPlaceholder = "--Conf--";
    
    // The number of years covered by the FCE
    public const int FceDataPeriod = 1; // One year
    
    // The number of years of additional data retrieved
    // (fees history and enforcement history)
    public const int FceExtendedDataPeriod = 5; // Five years
}
