using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

[SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty",
    Justification = "This record type triggers a different partial view in the UI.")]
public record ProposedCoViewDto : ResponseRequestedViewDto;
