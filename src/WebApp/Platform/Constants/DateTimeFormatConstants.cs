namespace AirWeb.WebApp.Platform.Constants;

internal static class DateTimeFormatConstants
{
    public const string LongDateTimeFormat = "MMMM\u00a0d, yyyy h:mm\u00a0tt";
    public const string LongDateFormat = "MMMM\u00a0d, yyyy";
    public const string ShortDateFormat = "d\u2011MMM\u2011yyyy";
    public const string ShortDateTimeFormat = "d\u2011MMM\u2011yyyy h:mm\u00a0tt";
    public const string ShortDateTimeNoBreakFormat = "d\u2011MMM\u2011yyyy\u00a0h:mm\u00a0tt";

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
