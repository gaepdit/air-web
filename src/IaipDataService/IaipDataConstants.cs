namespace IaipDataService;

public static class IaipDataConstants
{
    // Organization
    public const string EpdDirector = "A. Director";

    // Stack Tests
    public const string ConfidentialInfoPlaceholder = "--Conf--";

    public const string ReportStatement = "The following test has been reviewed and was conducted " +
                                          "in an acceptable fashion for the purpose intended.";

    // FCEs
    // The number of years covered by the FCE
    public const int FceDataPeriod = 1; // One year

    // The number of years of additional data retrieved
    // (fees history and enforcement history)
    public const int FceExtendedDataPeriod = 5; // Five years
}
