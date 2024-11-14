namespace AirWeb.Domain.EnforcementEntities.Cases;

public record ViolationType(
    [StringLength(3)] string Code,
    string Description,
    [StringLength(3)] string SeverityCode,
    bool Deprecated
);
