namespace AirWeb.Domain.Core.Data.DataAttributes;

/// <summary>
/// Data attribute that specifies the maximum date value for a date property.
/// When applied to a date property in a DTO, this attribute causes the UI to set a max attribute
/// on the date input element.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MaxDateAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the MaxDateAttribute with a specific date.
    /// </summary>
    /// <param name="year">The year component of the maximum date.</param>
    /// <param name="month">The month component of the maximum date.</param>
    /// <param name="day">The day component of the maximum date.</param>
    public MaxDateAttribute(int year, int month, int day)
    {
        MaxDate = new DateOnly(year, month, day);
    }

    /// <summary>
    /// Initializes a new instance of the MaxDateAttribute with the current date as the maximum.
    /// </summary>
    public MaxDateAttribute()
    {
        UseTodayAsMax = true;
    }

    /// <summary>
    /// Gets the maximum date value.
    /// </summary>
    public DateOnly? MaxDate { get; }

    /// <summary>
    /// Gets a value indicating whether to use today's date as the maximum.
    /// </summary>
    public bool UseTodayAsMax { get; }
}
