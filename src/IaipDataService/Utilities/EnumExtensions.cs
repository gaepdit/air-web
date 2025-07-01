using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IaipDataService.Utilities;

/// <summary>
/// Enumeration type extension methods.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets an attribute on an enum field value.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
    /// <typeparam name="TEnum">The Enum type of the enum value.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <param name="enumString">The string representation of the enum value.</param>
    /// <returns>
    /// The attribute of the specified type or null.
    /// </returns>
    private static T? GetAttributeOfType<T, TEnum>(this TEnum enumValue, string enumString)
        where T : Attribute where TEnum : Enum
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumString)[0];
        var attributes = memInfo.GetCustomAttributes<T>(false);
        return attributes.FirstOrDefault();
    }

    /// <summary>
    /// Gets the enum display name.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Use <see cref="DisplayAttribute"/> if exists.
    /// Otherwise, use the standard string representation.
    /// </returns>
    public static string GetDisplayName<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var enumString = enumValue.ToString();
        var attribute = enumValue.GetAttributeOfType<DisplayAttribute, TEnum>(enumString);
        return attribute?.Name ?? enumString;
    }

    /// <summary>
    /// Gets the enum description.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// Use <see cref="DescriptionAttribute"/> if exists.
    /// Otherwise, use the standard string representation.
    /// </returns>
    public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var enumString = enumValue.ToString();
        var attribute = enumValue.GetAttributeOfType<DescriptionAttribute, TEnum>(enumString);
        return attribute?.Description ?? enumString;
    }
}
