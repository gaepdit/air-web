namespace AirWeb.Domain.EnforcementEntities.ViolationTypes;

public record ViolationType(
    [property: Key] [property: StringLength(5)] string Code,
    string Description,
    [property: StringLength(3)] string SeverityCode,
    bool Deprecated);
