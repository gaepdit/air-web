using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Utilities;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;

public abstract record PermitRevocationCommandDto : ComplianceWorkCommandDto, IPermitRevocationCommandDto
{
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Permit Revocation Date")]
    public DateOnly PermitRevocationDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Physical Shutdown Date")]
    public DateOnly? PhysicalShutdownDate { get; init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; init; }
}
