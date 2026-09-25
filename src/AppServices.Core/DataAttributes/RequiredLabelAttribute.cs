namespace AirWeb.AppServices.Core.DataAttributes;

/// <summary>
/// Data attribute that indicates that a property should be labeled as required in the UI.
/// By default, the required field indicator is excluded from enums and booleans as this generally
/// adds noise to the UI. Adding a `[RequiredLabel]` attribute forces the display of the label.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredLabelAttribute : Attribute;

/// <summary>
/// Data attribute that indicates that a property should NOT be labeled as required in the UI (even if it is, in fact,
/// required). By default, the required field indicator is included for required properties. If this ends up adding
/// unuseful noise to a particular UI, adding a `[RequiredNoLabel]` attribute suppresses the display of the label.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredNoLabelAttribute : Attribute;
