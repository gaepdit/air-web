using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.Command;

public record CaseFileCreateDto
{
    public CaseFileCreateDto() { }

    public CaseFileCreateDto(FacilityId facilityId, string userId)
    {
        FacilityId = facilityId;
        ResponsibleStaffId = userId;
    }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Required]
    public DateOnly? DiscoveryDate { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
