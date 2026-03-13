namespace AirWeb.AppServices.Core.DataAttributes;

/// <summary>
/// Data attribute that indicates that a property should be labeled as required in the UI.
/// By default, the required field indicator is excluded from enums and booleans as this generally
/// adds noise to the UI. Adding a `RequiredLabelAttribute` forces the display of the label.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredLabelAttribute : Attribute;
