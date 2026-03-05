namespace AirWeb.Domain.Core.Data.DataAttributes;

/// <summary>
/// Data attribute that specifies the maximum date value for a date property.
/// When applied to a date property in a DTO, this attribute causes the UI to set a max attribute
/// on the date input element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MaxDateAttribute : Attribute
{
    public const string HtmlInputDate = "yyyy-MM-dd";

    /// <summary>
    /// Initializes a new instance of the MaxDateAttribute with a specific date.
    /// </summary>
    /// <param name="year">The year component of the maximum date.</param>
    /// <param name="month">The month component of the maximum date.</param>
    /// <param name="day">The day component of the maximum date.</param>
    public MaxDateAttribute(int year, int month, int day) => MaxDate = new DateOnly(year, month, day);

    /// <summary>
    /// Initializes a new instance of the MaxDateAttribute with the maximum date set by the number of days from today.
    /// </summary>
    public MaxDateAttribute(int daysFromToday) =>
        MaxDate = DateOnly.FromDateTime(DateTime.Today.AddDays(daysFromToday));

    /// <summary>
    /// Initializes a new instance of the MaxDateAttribute with the current date as the maximum.
    /// </summary>
    public MaxDateAttribute() => MaxDate = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    /// Gets the maximum date value.
    /// </summary>
    public DateOnly? MaxDate { get; }

    /// <summary>
    /// Gets the maximum date value as a string formatted for the HTML date input "max" attribute.
    /// </summary>
    public string? MaxDateValue => MaxDate?.ToString(HtmlInputDate);
}
