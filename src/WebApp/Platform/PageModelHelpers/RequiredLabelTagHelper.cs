using AirWeb.AppServices.Core.DataAttributes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AirWeb.WebApp.Platform.PageModelHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting <c>&lt;label&gt;</c> elements with an <c>asp-for</c> attribute.
/// </summary>
[HtmlTargetElement("label", Attributes = ForAttributeName)]
public class RequiredLabelTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [UsedImplicitly]
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For
    {
        set;
        get => field ?? throw new InvalidOperationException("Uninitialized Model.");
    }

    /// <inheritdoc />
    /// <remarks>
    /// Adds text indicating the field is required if the property has the RequiredAttribute or RequiredLabelAttribute.
    /// </remarks>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Don't label fields that are not required.
        if (!For.Metadata.IsRequired) return;

        // Don't label boolean or enum-based fields unless the property has the RequiredLabelAttribute.
        if ((For.Metadata.ModelType == typeof(bool) || For.Metadata.ModelType.IsEnum) &&
            For.Metadata.ContainerType?
                .GetProperty(For.Metadata.PropertyName ?? string.Empty)?
                .GetCustomAttributes(typeof(RequiredLabelAttribute), inherit: true)
                .FirstOrDefault() is not RequiredLabelAttribute)
            return;

        output.Content.AppendHtml(""" <span title="Required" class="text-danger-emphasis">*</span>""");
    }
}
