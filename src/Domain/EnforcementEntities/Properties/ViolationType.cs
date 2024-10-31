namespace AirWeb.Domain.EnforcementEntities.Properties;

public record ViolationType(
    [StringLength(3)] string Code,
    string Description,
    [StringLength(3)] string SeverityCode,
    bool Deprecated
);
