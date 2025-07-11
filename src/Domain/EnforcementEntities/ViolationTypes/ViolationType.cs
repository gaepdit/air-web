﻿namespace AirWeb.Domain.EnforcementEntities.ViolationTypes;

public record ViolationType
{
    internal ViolationType() { }

    [Key]
    [StringLength(5)]
    public required string Code { get; init; }

    public required string Description { get; init; }

    [StringLength(3)]
    public required string SeverityCode { get; init; }

    public bool Deprecated { get; init; }

    public string Display => $"{SeverityCode}: {Description} ({Code})";
}
