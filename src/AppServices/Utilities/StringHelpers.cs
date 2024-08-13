namespace AirWeb.AppServices.Utilities;

public static class StringHelpers
{
    public static string CleanFacilityId(this string input) =>
        new(input.Where(c => c == '-' || char.IsDigit(c)).ToArray());
}
