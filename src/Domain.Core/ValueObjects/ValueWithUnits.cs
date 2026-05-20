namespace AirWeb.Domain.Core.ValueObjects;

public readonly record struct ValueWithUnits(
    string Value,
    string Units,
    string? Preamble = null
)
{
    public override string ToString() =>
        string.Join(
            Units == "%" ? "" : " ",
            new[] { Preamble, Value, Units }.Where(s => !string.IsNullOrEmpty(s)));
}
