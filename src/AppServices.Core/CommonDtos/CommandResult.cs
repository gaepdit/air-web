using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.Core.CommonDtos;

// Used for modifying entities. Can include non-failure warning messages.
public record CommandResult
{
    protected CommandResult() { }

    public string? WarningMessage { get; protected init; }

    [MemberNotNullWhen(true, nameof(WarningMessage))]
    public bool HasWarning => WarningMessage != null;

    // Static constructors
    public static CommandResult Create(string? warningMessage = null) => new() { WarningMessage = warningMessage };
}
