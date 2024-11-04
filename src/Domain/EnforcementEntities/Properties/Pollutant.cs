namespace AirWeb.Domain.EnforcementEntities.Properties;

public record Pollutant(
    [StringLength(9)] string Code,
    string Description
);
