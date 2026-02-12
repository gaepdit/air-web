using System.Globalization;

namespace AirWeb.Core.Data.DataAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PositiveDecimalAttribute : RangeAttribute
{
    /// <summary>
    /// Creates a decimal range with zero as the minimum value and the decimal `MaxValue` as the maximum value.
    /// </summary>
    public PositiveDecimalAttribute()
        : base(typeof(decimal), minimum: "0", maximum: decimal.MaxValue.ToString(CultureInfo.CurrentCulture))
    {
        ErrorMessage = "Value must be greater than {0}.";
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PositiveShortAttribute : RangeAttribute
{
    /// <summary>
    /// Creates a short integer range with zero as the minimum value and the short `MaxValue` as the maximum value.
    /// </summary>
    public PositiveShortAttribute()
        : base(typeof(short), minimum: "0", maximum: short.MaxValue.ToString(CultureInfo.CurrentCulture))
    {
        ErrorMessage = "Value must be greater than {0}.";
    }
}
