using AirWeb.AppServices.Core.Utilities;
using AirWeb.Domain.Core.Data.DataAttributes;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AirWeb.WebApp.Platform.PageModelHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting <c>&lt;input&gt;</c> elements with an <c>asp-for</c> attribute
/// and a type of "date". This tag helper reads the <see cref="MaxDateAttribute"/> from the model property and sets
/// the max attribute on the input element.
/// </summary>
[HtmlTargetElement("input", Attributes = ForAttributeName)]
public class MaxDateTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";

    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression? For { get; set; }

    /// <inheritdoc />
    /// <remarks>
    /// Reads the MaxDateAttribute from the model property and sets the max attribute on date input elements.
    /// </remarks>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Only process input elements with type="date" or date-related inputs
        if (For == null || output.TagName != "input")
            return;

        // Check if the property has the MaxDateAttribute
        var maxDateAttribute = For.Metadata.ContainerType?
            .GetProperty(For.Metadata.PropertyName ?? string.Empty)?
            .GetCustomAttributes(typeof(MaxDateAttribute), inherit: true)
            .FirstOrDefault() as MaxDateAttribute;

        if (maxDateAttribute == null)
            return;

        // Only set max if it's not already set (allow explicit overrides)
        if (output.Attributes.ContainsName("max"))
            return;

        // Determine the max date value
        var maxDateValue = maxDateAttribute.UseTodayAsMax
            ? DateTime.Today.ToString(DateTimeFormats.HtmlInputDate)
            : maxDateAttribute.MaxDate?.ToString(DateTimeFormats.HtmlInputDate);

        if (maxDateValue != null)
            output.Attributes.SetAttribute("max", maxDateValue);
    }
}
