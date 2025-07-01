namespace AirWeb.WebApp.Platform.PageModelHelpers;

public static class EnumHelper
{
    public static IEnumerable<SelectListItem> GetEnumSelectListWithDescription<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => new SelectListItem
        {
            Value = e.ToString(),
            Text = e.GetDescription(),
        });
    }
}
