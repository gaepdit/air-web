namespace AirWeb.AppServices.Utilities;

public static class DateTimeFormats
{
    // Format strings
    public const string LongDateTime = "MMMM\u00a0d, yyyy h:mm\u00a0tt";
    public const string LongDate = "MMMM\u00a0d, yyyy";
    public const string ShortDate = "d\u2011MMM\u2011yyyy";
    public const string ShortDateTime = "d\u2011MMM\u2011yyyy h:mm\u00a0tt";
    public const string ShortDateTimeNoBreak = "d\u2011MMM\u2011yyyy\u00a0h:mm\u00a0tt";
    public const string HtmlInputDate = "yyyy-MM-dd";

    // Composite format strings
    public const string DateOnlyInput = "{0:yyyy-MM-dd}";

    // Extension methods

    /// <summary>
    ///     DateTime extension method to display a nullable DateTime as a string or display a replacement string
    ///     if the value is null.
    /// </summary>
    /// <param name="dt">The nullable DateTime to display.</param>
    /// <param name="format">The format for displaying the DateTime if it is not null.</param>
    /// <param name="nullReplacement">The replacement string to display if the DateTime is null.</param>
    public static string ToString(this DateTime? dt, string format, string nullReplacement = "N/A") =>
        dt == null ? nullReplacement : dt.Value.ToString(format);

    /// <summary>
    ///     DateOnly extension method to display a nullable DateOnly as a string or display a replacement string
    ///     if the value is null.
    /// </summary>
    /// <param name="dt">The nullable DateOnly to display.</param>
    /// <param name="format">The format for displaying the DateOnly if it is not null.</param>
    /// <param name="nullReplacement">The replacement string to display if the DateOnly is null.</param>
    public static string ToString(this DateOnly? dt, string format, string nullReplacement = "N/A") =>
        dt == null ? nullReplacement : dt.Value.ToString(format);
}
